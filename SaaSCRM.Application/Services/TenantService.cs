using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;
using System;
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
    }
}
