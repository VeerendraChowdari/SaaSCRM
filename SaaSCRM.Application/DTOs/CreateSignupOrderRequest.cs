
namespace SaaSCRM.Application.DTOs
{
    public class CreateSignupOrderRequest
    {
        // Company details
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Admin details
        public string AdminEmail { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;

        // Plan selected by the user
        public string PlanName { get; set; } = string.Empty;
    }
}
