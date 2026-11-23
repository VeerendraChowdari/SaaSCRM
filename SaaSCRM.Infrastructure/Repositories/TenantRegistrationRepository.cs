
using Microsoft.EntityFrameworkCore;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;
using SaaSCRM.Infrastructure.Persistence;

namespace SaaSCRM.Infrastructure.Repositories
{
    public class TenantRegistrationRepository : ITenantRegistrationRepository
    {
        private readonly ApplicationDbContext _context;

        public TenantRegistrationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CompanyExistsAsync(string companyName)
        {
            return await _context.Tenants
                .AnyAsync(t => t.CompanyName == companyName);
        }

        public async Task<bool> AdminEmailExistsAsync(string adminEmail)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == adminEmail);
        }
        public async Task<bool> SlugExistsAsync(string slug)
        {
            return await _context.Tenants
                .AnyAsync(t => t.Slug == slug);
        }

        public async Task CreateTenantAndAdminAsync(
            Tenant tenant,
            User adminUser,
            PendingRegistration pendingRegistration,
            Payment payment)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Tenants.AddAsync(tenant);
                await _context.Users.AddAsync(adminUser);

                pendingRegistration.Status = "Completed";
                pendingRegistration.CompletedOn = DateTime.UtcNow;

                payment.Status = "PAID";

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
