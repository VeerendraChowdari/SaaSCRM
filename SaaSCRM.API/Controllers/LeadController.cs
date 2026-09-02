using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaSCRM.Application.DTOs;
using SaaSCRM.Application.Services;

namespace SaaSCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeadController : ControllerBase
    {
        private readonly LeadService _leadService;

        public LeadController(LeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpPost]
        public async Task<IActionResult> AddLeadAsync(CreateLeadRequest request)
        {
            var result = await _leadService.AddLeadAsync(request);

            if (result == "Lead already exists")
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLeadsAsync()
        {
            var result = await _leadService.GetAllLeadsAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLeadByIdAsync(Guid id)
        {
            var result = await _leadService.GetLeadByIdAsync(id);

            if (result == null)
                return NotFound("Lead not found.");

            return Ok(result);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetLeadByEmailAsync(string email)
        {
            var result = await _leadService.GetLeadByEmailAsync(email);

            if (result == null)
                return NotFound("Lead not found.");

            return Ok(result);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetLeadByNameAsync(string name)
        {
            var result = await _leadService.GetLeadByNameAsync(name);

            if (result == null)
                return NotFound("Lead not found.");

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeadAsync(
            Guid id,
            UpdateLeadRequest request)
        {
            var existingLead = await _leadService.GetLeadByIdAsync(id);

            if (existingLead == null)
                return NotFound("Lead not found.");

            await _leadService.UpdateLeadAsync(id, request);

            return Ok("Lead updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeadAsync(Guid id)
        {
            var result = await _leadService.DeleteLeadAsync(id);

            if (!result)
                return NotFound("Lead not found.");

            return Ok("Lead deleted successfully.");
        }
    }
}