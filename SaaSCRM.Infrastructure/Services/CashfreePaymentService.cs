
using System.Net.Http.Json;
using System.Text.Json;
using SaaSCRM.Application.DTOs;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Infrastructure.Configurations;

namespace SaaSCRM.Infrastructure.Services
{
    public class CashfreePaymentService : ICashfreePaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly CashfreeSettings _settings;

        public CashfreePaymentService(
            HttpClient httpClient,
            Microsoft.Extensions.Options.IOptions<CashfreeSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<CreatePaymentOrderResponse> CreateOrderAsync(
            CreatePaymentOrderRequest request)
        {
            if (request.Amount <= 0)
            {
                throw new ArgumentException(
                    "Payment amount must be greater than zero.");
            }

            var orderId = $"saascrm_{Guid.NewGuid():N}";

            var orderRequest = new
            {
                order_id = orderId,
                order_amount = request.Amount,
                order_currency = "INR",

                customer_details = new
                {
                    customer_id = Guid.NewGuid().ToString("N"),
                    customer_name = request.CustomerName,
                    customer_email = request.CustomerEmail,
                    customer_phone = request.CustomerPhone
                },

                order_meta = new
                {
                    return_url =
                        "http://localhost:5173/payment-result?order_id={order_id}"
                }
            };

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "https://sandbox.cashfree.com/pg/orders");

            httpRequest.Headers.Add("x-client-id", _settings.AppId);
            httpRequest.Headers.Add("x-client-secret", _settings.SecretKey);
            httpRequest.Headers.Add("x-api-version", "2025-01-01");

            httpRequest.Content = JsonContent.Create(orderRequest);

            using var response = await _httpClient.SendAsync(httpRequest);

            var responseBody =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Cashfree order creation failed " +
                    $"({(int)response.StatusCode}): {responseBody}");
            }

            using var document = JsonDocument.Parse(responseBody);

            if (!document.RootElement.TryGetProperty(
                    "payment_session_id", out var sessionId))
            {
                throw new InvalidOperationException(
                    "Cashfree did not return a payment session ID.");
            }

            var paymentSessionId = sessionId.GetString()
                ?? throw new InvalidOperationException(
                    "Cashfree returned an empty payment session ID.");

            return new CreatePaymentOrderResponse
            {
                OrderId = orderId,
                PaymentSessionId = paymentSessionId
            };
        }

        public async Task<PaymentStatusResponse> GetOrderStatusAsync(
            string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentException("Order ID is required.");
            }

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://sandbox.cashfree.com/pg/orders/{Uri.EscapeDataString(orderId)}");

            httpRequest.Headers.Add("x-client-id", _settings.AppId);
            httpRequest.Headers.Add("x-client-secret", _settings.SecretKey);
            httpRequest.Headers.Add("x-api-version", "2025-01-01");

            using var response = await _httpClient.SendAsync(httpRequest);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Cashfree status check failed " +
                    $"({(int)response.StatusCode}): {responseBody}");
            }

            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;

            return new PaymentStatusResponse
            {
                OrderId = root.TryGetProperty(
                    "order_id", out var orderIdElement)
                    ? orderIdElement.GetString() ?? orderId
                    : orderId,

                Status = root.TryGetProperty(
                    "order_status", out var statusElement)
                    ? statusElement.GetString() ?? "UNKNOWN"
                    : "UNKNOWN",

                Amount = root.TryGetProperty(
                    "order_amount", out var amountElement)
                    ? amountElement.GetDecimal()
                    : 0,

                Currency = root.TryGetProperty(
                    "order_currency", out var currencyElement)
                    ? currencyElement.GetString() ?? "INR"
                    : "INR"
            };
        }
    }
}
