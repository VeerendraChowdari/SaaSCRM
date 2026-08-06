using Microsoft.AspNetCore.Mvc;
using SaaSCRM.Application.Services;
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly TenantService _tenantService;
        public TenantController(TenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant(Tenant tenant)
        {
            var result= await _tenantService.CreateTenant(tenant);
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

        [HttpGet("CompanyName/{CompanyName}")]
        public async Task<IActionResult> GetByCompanyNameAsync(String CompanyName)
        {
            var res = await _tenantService.GetByCompanyNameAsync(CompanyName);
            return Ok(res);
        }

        [HttpGet("emailid/{emailid}")]
        public async Task<IActionResult> GetByEmailAsync(string emailid)
        {
            var res = await _tenantService.GetByEmailAsync(emailid);
            return Ok(res);
        }
    }
}
