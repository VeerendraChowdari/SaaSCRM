using Microsoft.EntityFrameworkCore;
using SaaSCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SaaSCRM.Infrastructure.Persistence
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Tenant>Tenants { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
