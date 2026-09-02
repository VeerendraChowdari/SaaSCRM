using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSCRM.Domain.Entities
{
    public class Lead
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public Guid? AssignedToUserId { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public Tenant Tenant { get; set; } = null!;

        public User? AssignedToUser { get; set; }
    }
}
