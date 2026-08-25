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
        public async Task<IActionResult> AddAsync(CreateUserRequest request)
        {
            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = request.Email,
                Phoneno = "0000000000",
                PasswordHash = request.Password,
                UserRole = "User"
            };
            var res = await _UserService.AddAsync(user);

            return Ok(res);
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var res = await _UserService.GetAllAsync();

            var response = res.Select(user => new UserResponseDto
            {
                Id = user.Id,
                TenantId = user.TenantId,
                Email = user.Email,
                UserRole = user.UserRole
            });

            return Ok(response);
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

            var response = new UserResponseDto
            {
                Id = res.Id,
                TenantId = res.TenantId,
                Email = res.Email,
                UserRole = res.UserRole
            };
            return Ok(response);
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
            var response = new UserResponseDto
            {
                Id = res.Id,
                TenantId = res.TenantId,
                Email = res.Email,
                UserRole = res.UserRole
            };
            return Ok(response);
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateUserRequest request)
        {
            var existingUser = await _UserService.GetByIdAsync(id);

            if (existingUser == null)
            {
                return NotFound("User not found.");
            }

            existingUser.Email = request.Email;

            var res = await _UserService.UpdateAsync(existingUser);

            return Ok(res);
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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