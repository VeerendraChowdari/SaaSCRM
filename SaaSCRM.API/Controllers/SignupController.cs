
using Microsoft.AspNetCore.Mvc;
using SaaSCRM.Application.DTOs;
using SaaSCRM.Application.Services;

namespace SaaSCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly SignupPaymentService _signupPaymentService;
        private readonly ILogger<SignupController> _logger;

        public SignupController(
            SignupPaymentService signupPaymentService,
            ILogger<SignupController> logger)
        {
            _signupPaymentService = signupPaymentService;
            _logger = logger;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(
            [FromBody] CreateSignupOrderRequest request)
        {
            try
            {
                var order =
                    await _signupPaymentService.CreateSignupOrderAsync(request);

                return Ok(new
                {
                    orderId = order.OrderId,
                    paymentSessionId = order.PaymentSessionId
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Cashfree signup order creation failed.");

                return StatusCode(
                    502,
                    new { message = "Unable to create a payment order." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Signup order creation failed.");

                return StatusCode(
                    500,
                    new { message = "Unable to start signup." });
            }
        }

        [HttpPost("activate/{orderId}")]
        public async Task<IActionResult> ActivateSignup(string orderId)
        {
            try
            {
                var result =
                    await _signupPaymentService.ActivateSignupAsync(orderId);

                return Ok(new { message = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(
                    ex,
                    "Cashfree payment verification failed for order {OrderId}.",
                    orderId);

                return StatusCode(
                    502,
                    new { message = "Unable to verify payment right now." });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Signup activation failed for order {OrderId}.",
                    orderId);

                return StatusCode(
                    500,
                    new { message = "Unable to activate signup." });
            }
        }
    }
}
