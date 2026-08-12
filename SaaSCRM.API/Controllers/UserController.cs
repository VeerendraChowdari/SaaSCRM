using SaaSCRM.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using SaaSCRM.Application.Services;
using SaaSCRM.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace SaaSCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _UserService;

        public UserController(UserService userService)
        {
            _UserService = userService;
        }

        // POST: api/User
        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> AddAsync(User user)
        {
            var res = await _UserService.AddAsync(user);

            return Ok(res);
        }

        // GET: api/User
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync()
        {
            var res = await _UserService.GetAllAsync();

            return Ok(res);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var res = await _UserService.DeleteAsync(id);

            if (res == "User not found.")
            {
                return NotFound(res);
            }

            return Ok(res);
        }


        // POST: api/User
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            var res = await _UserService.LoginAsync(request);

            return Ok(res);
        }
    }
}