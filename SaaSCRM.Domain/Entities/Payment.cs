
namespace SaaSCRM.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string OrderId { get; set; } = string.Empty;

        public string PaymentSessionId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "INR";

        public string Status { get; set; } = "Pending";

        public string CustomerEmail { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
