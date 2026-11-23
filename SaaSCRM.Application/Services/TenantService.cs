using SaaSCRM.Application.DTOs;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.Application.Services
{
    public class TenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public TenantService(
            ITenantRepository tenantRepository,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _tenantRepository = tenantRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> RegisterTenantAsync(
            RegisterTenantRequest request)
        {
            var exists = await _tenantRepository
                .ExistsAsync(request.CompanyName);

            if (exists)
            {
                return "Company already exists.";
            }

            var existingUser = await _userRepository
                .GetByEmailAsync(request.AdminEmail);

            if (existingUser != null)
            {
                return "Admin email already exists.";
            }

            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                CompanyName = request.CompanyName,
                Email = request.CompanyEmail,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };

            await _tenantRepository.AddAsync(tenant);

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Email = request.AdminEmail,
                PasswordHash = request.AdminPassword,
                UserRole = "Admin",
                TenantId = tenant.Id,
                FirstName = "Admin",
                LastName = "User",
                Phoneno = request.PhoneNumber
            };

            adminUser.PasswordHash =
                _passwordHasher.HashPassword(
                    adminUser,
                    adminUser.PasswordHash);

            await _userRepository.AddAsync(adminUser);

            return "Tenant and Admin created successfully.";
        }

        public async Task<string> CreateTenant(Tenant tenant)
        {
            var exists = await _tenantRepository
                .ExistsAsync(tenant.CompanyName);

            if (exists)
            {
                return "Company already exists.";
            }

            await _tenantRepository.AddAsync(tenant);

            return "Tenant created successfully.";
        }

        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return await _tenantRepository.GetAllAsync();
        }

        public async Task<Tenant?> GetByIdAsync(Guid id)
        {
            return await _tenantRepository.GetByIdAsync(id);
        }

        public async Task<Tenant?> GetByCompanyNameAsync(string companyName)
        {
            return await _tenantRepository
                .GetByCompanyNameAsync(companyName);
        }

        public async Task<Tenant?> GetByEmailAsync(string emailid)
        {
            return await _tenantRepository
                .GetByEmailAsync(emailid);
        }
        public async Task<Tenant?> GetBySlugAsync(string slug)
        {
            return await _tenantRepository.GetBySlugAsync(slug);
        }
    }
}