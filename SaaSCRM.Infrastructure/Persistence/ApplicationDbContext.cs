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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tenant>()
                .HasIndex(t => t.Slug)
                .IsUnique();
        }
        public DbSet<Customer> Customers{get;set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PendingRegistration> PendingRegistrations { get; set; }
    }
}
