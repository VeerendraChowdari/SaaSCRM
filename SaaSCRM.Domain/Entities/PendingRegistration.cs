
namespace SaaSCRM.Domain.Entities
{
    public class PendingRegistration
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string OrderId { get; set; } = string.Empty;

        // Company details
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Admin details
        public string AdminEmail { get; set; } = string.Empty;
        public string AdminPasswordHash { get; set; } = string.Empty;

        // Subscription details
        public string PlanName { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedOn { get; set; }
    }
}
