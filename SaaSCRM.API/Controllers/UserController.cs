using Microsoft.AspNetCore.Mvc;
using SaaSCRM.Application.Services;
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _UserService;

        public UserController(UserService userService)
        {
            _UserService = userService;
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> AddAsync(User user)
        {
            var res = await _UserService.AddAsync(user);

            return Ok(res);
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var res = await _UserService.GetAllAsync();

            return Ok(res);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var res = await _UserService.GetByIdAsync(id);

            if (res == null)
            {
                return NotFound("User not found.");
            }

            return Ok(res);
        }

        // GET: api/User/email/{email}
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            var res = await _UserService.GetByEmailAsync(email);

            if (res == null)
            {
                return NotFound("User not found.");
            }

            return Ok(res);
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var res = await _UserService.UpdateAsync(user);

            if (res == "User not found.")
            {
                return NotFound(res);
            }

            return Ok(res);
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var res = await _UserService.DeleteAsync(id);

            if (res == "User not found.")
            {
                return NotFound(res);
            }

            return Ok(res);
        }
    }
}