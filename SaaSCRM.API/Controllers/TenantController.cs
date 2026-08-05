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
    }
}
