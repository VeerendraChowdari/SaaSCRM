
using Microsoft.AspNetCore.Mvc;
using SaaSCRM.Application.DTOs;
using SaaSCRM.Application.Services;

namespace SaaSCRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicEnquiryController : ControllerBase
    {
        private readonly TenantService _tenantService;
        private readonly LeadService _leadService;

        public PublicEnquiryController(
            TenantService tenantService,
            LeadService leadService)
        {
            _tenantService = tenantService;
            _leadService = leadService;
        }

        // Get company details using tenant slug
        [HttpGet("tenant/{slug}")]
        public async Task<IActionResult> GetTenantBySlug(string slug)
        {
            var tenant = await _tenantService.GetBySlugAsync(slug);

            if (tenant == null || !tenant.IsActive)
            {
                return NotFound(new
                {
                    message = "Company not found or inactive."
                });
            }

            return Ok(new
            {
                tenant.CompanyName,
                tenant.Slug
            });
        }

        // Submit public enquiry
        [HttpPost("tenant/{slug}")]
        public async Task<IActionResult> SubmitEnquiry(
            string slug,
            [FromBody] CreatePublicEnquiryRequest request)
        {
            var tenant = await _tenantService.GetBySlugAsync(slug);

            if (tenant == null || !tenant.IsActive)
            {
                return NotFound(new
                {
                    message = "Company not found or inactive."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Phone))
            {
                return BadRequest(new
                {
                    message = "Name, email and phone are required."
                });
            }

            var result = await _leadService.AddPublicEnquiryAsync(
                tenant.Id,
                request);

            if (result == "Lead already exists")
            {
                return Conflict(new { message = result });
            }

            return Ok(new { message = result });
        }
    }
}
