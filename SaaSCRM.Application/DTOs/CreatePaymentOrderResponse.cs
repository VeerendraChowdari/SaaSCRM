namespace SaaSCRM.Application.DTOs
{
    public class CreatePaymentOrderResponse
    {
        public string OrderId { get; set; } = string.Empty;

        public string PaymentSessionId { get; set; } = string.Empty;
    }
}