using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSCRM.Application.Services
{
    public class TenantService
    {
        private readonly ITenantRepository _tenantRepository;
        public TenantService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<string> CreateTenant(Tenant tenant)
        {
            var exists = await _tenantRepository.ExistsAsync(tenant.CompanyName);
            if (exists)
            {
                return "Company already exists.";
            }
            await _tenantRepository.AddAsync(tenant);
            return "Tenant created successfully.";
        }

        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return  await _tenantRepository.GetAllAsync();
        }

        public async Task<Tenant?> GetByIdAsync( Guid id)
        {
            return await _tenantRepository.GetByIdAsync(id);
        }

        public async Task<Tenant?> GetByCompanyNameAsync(string CompanyName)
        {
            return await _tenantRepository.GetByCompanyNameAsync(CompanyName);
        }
        public async Task<Tenant?> GetByEmailAsync(string emailid)
        {
            return await _tenantRepository.GetByEmailAsync(emailid);
        }
    }
}
