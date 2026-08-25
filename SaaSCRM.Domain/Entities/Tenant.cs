    namespace SaaSCRM.Domain.Entities;
    public class Tenant
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    }