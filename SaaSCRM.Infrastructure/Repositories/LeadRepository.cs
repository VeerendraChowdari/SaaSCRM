using Microsoft.EntityFrameworkCore;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;
using SaaSCRM.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSCRM.Infrastructure.Repositories
{
    public class LeadRepository:ILeadRepository
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
                .FirstOrDefaultAsync(x => x.Id == lead.Id);

            if (existingLead != null)
            {
                _context.Leads.Remove(existingLead);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Lead>> GetAllLeadsAsync(Guid tenantId)
        {
            return await _context.Leads.Where(x => x.TenantId == tenantId).ToListAsync();
        }

        public async Task<Lead?> GetLeadByEmailAsync(string email)
        {
           return await _context.Leads.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<Lead?> GetLeadByIdAsync(Guid id)
        {
            return _context.Leads.FirstOrDefaultAsync(x =>x.Id == id);
        }

        public Task<Lead?> GetLeadByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateLeadAsync(Lead lead)
        {
            var existingLead = await _context.Leads
                .FirstOrDefaultAsync(x => x.Id == lead.Id);

            if (existingLead != null)
            {
                _context.Leads.Update(lead);
                await _context.SaveChangesAsync();
            }
        }
    }
}
