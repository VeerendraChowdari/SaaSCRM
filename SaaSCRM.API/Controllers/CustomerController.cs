using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaaSCRM.Application.DTOs;
using SaaSCRM.Application.Services;
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<IActionResult> AddAsync(CreateCustomerRequest request)
        {
            var customer = new Customer
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Address = request.Address
            };

            var res = await _customerService.AddAsync(customer);

            return Ok(res);
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var res = await _customerService.GetAllAsync();

            return Ok(res);
        }

        // GET: api/Customer/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var res = await _customerService.GetByIdAsync(id);

            if (res == null)
            {
                return NotFound("Customer not found.");
            }

            return Ok(res);
        }

        // GET: api/Customer/email/{email}
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            var res = await _customerService.GetByEmail(email);

            if (res == null)
            {
                return NotFound("Customer not found.");
            }

            return Ok(res);
        }

        // PUT: api/Customer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(
            Guid id,
            UpdateCustomerRequest request)
        {
            var existingCustomer = await _customerService.GetByIdAsync(id);

            if (existingCustomer == null)
            {
                return NotFound("Customer not found.");
            }

            existingCustomer.Name = request.Name;
            existingCustomer.Email = request.Email;
            existingCustomer.Phone = request.Phone;
            existingCustomer.Address = request.Address;

            await _customerService.UpdateAsync(existingCustomer);

            return Ok("Customer updated successfully.");
        }

        // DELETE: api/Customer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existingCustomer = await _customerService.GetByIdAsync(id);

            if (existingCustomer == null)
            {
                return NotFound("Customer not found.");
            }

            await _customerService.DeleteAsync(existingCustomer);

            return Ok("Customer deleted successfully.");
        }
    }
}