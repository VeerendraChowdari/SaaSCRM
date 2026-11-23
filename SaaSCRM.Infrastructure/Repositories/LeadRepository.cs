using Microsoft.EntityFrameworkCore;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;
using SaaSCRM.Infrastructure.Persistence;

namespace SaaSCRM.Infrastructure.Repositories
{
    public class LeadRepository : ILeadRepository
    {
        private readonly ApplicationDbContext _context;

        public LeadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddLeadAsync(Lead lead)
        {
            await _context.Leads.AddAsync(lead);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLeadAsync(Lead lead)
        {
            var existingLead = await _context.Leads
                .FirstOrDefaultAsync(x =>
                    x.Id == lead.Id &&
                    x.TenantId == lead.TenantId);

            if (existingLead != null)
            {
                _context.Leads.Remove(existingLead);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Lead>> GetAllLeadsAsync(Guid tenantId)
        {
            return await _context.Leads
                .Where(x => x.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task<Lead?> GetLeadByEmailAsync(
            string email,
            Guid tenantId)
        {
            return await _context.Leads
                .FirstOrDefaultAsync(x =>
                    x.Email == email &&
                    x.TenantId == tenantId);
        }

        public async Task<Lead?> GetLeadByIdAsync(
            Guid id,
            Guid tenantId)
        {
            return await _context.Leads
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.TenantId == tenantId);
        }

        public async Task<Lead?> GetLeadByNameAsync(
            string name,
            Guid tenantId)
        {
            return await _context.Leads
                .FirstOrDefaultAsync(x =>
                    x.Name == name &&
                    x.TenantId == tenantId);
        }

        public async Task UpdateLeadAsync(Lead lead)
        {
            var existingLead = await _context.Leads
                .FirstOrDefaultAsync(x =>
                    x.Id == lead.Id &&
                    x.TenantId == lead.TenantId);

            if (existingLead != null)
            {
                existingLead.Name = lead.Name;
                existingLead.CompanyName = lead.CompanyName;
                existingLead.Email = lead.Email;
                existingLead.Phone = lead.Phone;
                existingLead.Source = lead.Source;
                existingLead.Status = lead.Status;
                existingLead.AssignedToUserId = lead.AssignedToUserId;

                await _context.SaveChangesAsync();
            }
        }
    }
}