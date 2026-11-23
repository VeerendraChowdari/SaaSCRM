
using Microsoft.AspNetCore.Mvc;
using SaaSCRM.Application.DTOs;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ICashfreePaymentService _cashfreePaymentService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            ICashfreePaymentService cashfreePaymentService,
            IPaymentRepository paymentRepository,
            ILogger<PaymentController> logger)
        {
            _cashfreePaymentService = cashfreePaymentService;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        // POST: api/Payment/create-order
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreatePaymentOrderRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body is required.");
            }

            if (request.Amount <= 0)
            {
                return BadRequest("Amount must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(request.CustomerEmail))
            {
                return BadRequest("Customer email is required.");
            }

            try
            {
                // 1. Create an order with Cashfree.
                var result =
                    await _cashfreePaymentService.CreateOrderAsync(request);

                // 2. Save the order in PostgreSQL as Pending.
                var payment = new Payment
                {
                    OrderId = result.OrderId,
                    PaymentSessionId = result.PaymentSessionId,
                    Amount = request.Amount,
                    Currency = "INR",
                    CustomerEmail = request.CustomerEmail,
                    Status = "Pending"
                };

                await _paymentRepository.AddAsync(payment);
                await _paymentRepository.SaveChangesAsync();

                // 3. Return the details needed by the frontend.
                return Ok(new
                {
                    orderId = result.OrderId,
                    paymentSessionId = result.PaymentSessionId
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Cashfree order creation failed.");

                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    "Payment provider request failed.");
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Cashfree order creation timed out.");

                return StatusCode(
                    StatusCodes.Status504GatewayTimeout,
                    "Payment provider request timed out.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while creating payment order.");

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while creating the payment order.");
            }
        }

        // GET: api/Payment/status/{orderId}
        [HttpGet("status/{orderId}")]
        public async Task<IActionResult> GetPaymentStatus(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                return BadRequest("Order ID is required.");
            }

            try
            {
                // 1. Find the payment in our database.
                var payment =
                    await _paymentRepository.GetByOrderIdAsync(orderId);

                if (payment == null)
                {
                    return NotFound("Payment order was not found.");
                }

                // 2. Ask Cashfree for the latest order status.
                var status = await _cashfreePaymentService
                    .GetOrderStatusAsync(orderId);

                // 3. Check that the provider response matches our record.
                if (!string.Equals(
                        status.OrderId,
                        payment.OrderId,
                        StringComparison.Ordinal) ||
                    status.Amount != payment.Amount ||
                    !string.Equals(
                        status.Currency,
                        payment.Currency,
                        StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning(
                        "Cashfree order details did not match saved payment {OrderId}.",
                        orderId);

                    return StatusCode(
                        StatusCodes.Status502BadGateway,
                        "Payment provider returned mismatched order details.");
                }

                // 4. Update the saved status from Cashfree's response.
                payment.Status = status.Status;
                await _paymentRepository.SaveChangesAsync();

                return Ok(new
                {
                    orderId = payment.OrderId,
                    amount = payment.Amount,
                    currency = payment.Currency,
                    status = payment.Status
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(
                    ex,
                    "Cashfree status check failed for {OrderId}.",
                    orderId);

                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    "Could not verify payment status with the payment provider.");
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(
                    ex,
                    "Cashfree status check timed out for {OrderId}.",
                    orderId);

                return StatusCode(
                    StatusCodes.Status504GatewayTimeout,
                    "Payment status verification timed out.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected payment status error for {OrderId}.",
                    orderId);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred while checking payment status.");
            }
        }
    }
}
