namespace SaaSCRM.Application.DTOs
{
    public class CreateLeadRequest
    {
        public string Name { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Guid? AssignedToUserId { get; set; }
    }
}