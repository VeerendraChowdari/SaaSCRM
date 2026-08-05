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
            await _Context.AddAsync(tenant);
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

        public Task<IEnumerable<Tenant>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Tenant?> GetByCompanyNameAsync(string companyName)
        {
            throw new NotImplementedException();
        }

        public Task<Tenant?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Tenant?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Tenant tenant)
        {
            throw new NotImplementedException();
        }
    }
}
