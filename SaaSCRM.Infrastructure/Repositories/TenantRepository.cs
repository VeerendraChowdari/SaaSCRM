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
    public class TenantRepository : ITenantRepository
    {
        private readonly ApplicationDbContext _Context;

        public TenantRepository(ApplicationDbContext context)
        {
            _Context = context;
        }

        public async Task AddAsync(Tenant tenant)
        {
            await _Context.Tenants.AddAsync(tenant);
            await _Context.SaveChangesAsync();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(string companyName)
        {
            return await _Context.Tenants.AnyAsync(x => x.CompanyName == companyName);
        }

        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return await _Context.Tenants.ToListAsync();
        }

        public async Task<Tenant?> GetByCompanyNameAsync(string companyName)
        {
            return await _Context.Tenants.FirstOrDefaultAsync(x => x.CompanyName == companyName);
        }

        public async Task<Tenant?> GetByEmailAsync(string email)
        {
            return await _Context.Tenants.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Tenant?> GetByIdAsync(Guid id)
        {
            return await _Context.Tenants.FindAsync(id);
        }

        public async Task UpdateAsync(Tenant tenant)
        {
            var existingTenant = await _Context.Tenants.FindAsync(tenant.Id);
            if (existingTenant == null)
                return;
            existingTenant.CompanyName = tenant.CompanyName;
            existingTenant.Email = tenant.Email;
            existingTenant.PhoneNumber = tenant.PhoneNumber;
            existingTenant.Address = tenant.Address;
            existingTenant.IsActive = tenant.IsActive;

            await _Context.SaveChangesAsync();
        }
    }
}
