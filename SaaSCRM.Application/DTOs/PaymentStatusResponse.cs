namespace SaaSCRM.Application.DTOs
{
    public class PaymentStatusResponse
    {
        public string OrderId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "INR";
    }
}