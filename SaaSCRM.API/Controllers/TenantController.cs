
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SaaSCRM.Application.DTOs;
using SaaSCRM.Application.Services;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly TenantService _tenantService;
        private readonly ICurrentUserService _currentUser;

        public TenantController(
            TenantService tenantService,
            ICurrentUserService currentUser)
        {
            _tenantService = tenantService;
            _currentUser = currentUser;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterTenant(
            RegisterTenantRequest request)
        {
            var result = await _tenantService
                .RegisterTenantAsync(request);

            if (result == "Company already exists." ||
                result == "Admin email already exists.")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant(Tenant tenant)
        {
            var result = await _tenantService.CreateTenant(tenant);

            if (result == "Company already exists.")
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _tenantService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _tenantService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Tenant not found.");

            return Ok(result);
        }

        // NEW: Get the currently logged-in user's tenant
        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentTenant()
        {
            var tenant = await _tenantService
                .GetByIdAsync(_currentUser.TenantId);

            if (tenant == null)
                return NotFound("Tenant not found.");

            return Ok(new
            {
                tenant.CompanyName,
                tenant.Slug
            });
        }

        [HttpGet("CompanyName/{CompanyName}")]
        public async Task<IActionResult> GetByCompanyNameAsync(
            string CompanyName)
        {
            var result = await _tenantService
                .GetByCompanyNameAsync(CompanyName);

            return Ok(result);
        }

        [HttpGet("emailid/{emailid}")]
        public async Task<IActionResult> GetByEmailAsync(string emailid)
        {
            var result = await _tenantService
                .GetByEmailAsync(emailid);

            return Ok(result);
        }
    }
}
