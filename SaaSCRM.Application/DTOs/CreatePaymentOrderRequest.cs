namespace SaaSCRM.Application.DTOs
{
    public class CreatePaymentOrderRequest
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}