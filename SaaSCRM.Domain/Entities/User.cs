using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSCRM.Domain.Entities
{
    public class User
    {
        public  Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public  string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phoneno { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public string UserRole { get; set; }
        public DateTime CreatedOn { get; set; }
        public Tenant Tenant { get; set; } = null!;
     }
}
