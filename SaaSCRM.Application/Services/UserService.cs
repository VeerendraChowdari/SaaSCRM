using SaaSCRM.Application.DTOs;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace SaaSCRM.Application.Services
{
    public class UserService
    {
        public readonly IUserRepository _UserRepository;
        private readonly IPasswordHasher _PasswordHasher;
        private readonly ICurrentUserService _CurrentUser;
        private readonly IJwtService _JwtService;
        public UserService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtService ijwtservice,
    ICurrentUserService currentUser)
        {
            _UserRepository = userRepository;
            _PasswordHasher = passwordHasher;
            _JwtService = ijwtservice;
            _CurrentUser = currentUser;
        }
        public async Task<string> AddAsync(User user)
        {
            var res = await _UserRepository.GetByEmailAsync(user.Email);

            if (res != null)
            {
                return "User already exists with this email.";
            }
          user.PasswordHash = _PasswordHasher.HashPassword(user,user.PasswordHash);
            user.TenantId = _CurrentUser.TenantId;
            await _UserRepository.AddAsync(user);

            return "User created successfully.";
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _UserRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _UserRepository.GetAllAsync(_CurrentUser.TenantId);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _UserRepository.GetByEmailAsync(email);
        }

        public async Task<string> DeleteAsync(Guid id)
        {
            var existingUser = await _UserRepository.GetByIdAsync(id);

            if (existingUser == null)
            {
                return "User not found.";
            }

            await _UserRepository.DeleteAsync(id);

            return "User deleted successfully.";
        }

        public async Task<string> UpdateAsync(User user)
        {
            var existingUser = await _UserRepository.GetByIdAsync(user.Id);

            if (existingUser == null)
            {
                return "User not found.";
            }

            await _UserRepository.UpdateAsync(user);

            return "User updated successfully.";
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var ExistingEmail = await _UserRepository.GetByEmailAsync(request.Email);
            if (ExistingEmail == null)
                return "Invalid email or password.";

            var PasswordMatch =  _PasswordHasher.VerifyPassword(ExistingEmail, request.Password,ExistingEmail.PasswordHash);
            if (!PasswordMatch)
                return "Invalid email or password.";

            var token = _JwtService.GenerateToken(ExistingEmail);

            //return token;
            //var token = _JwtService.GenerateToken(ExistingEmail);

            //var handler = new JwtSecurityTokenHandler();
            //var jwtToken = handler.ReadJwtToken(token);

            //Console.WriteLine("===== JWT HEADER =====");
            //Console.WriteLine(jwtToken.Header.SerializeToJson());

            //Console.WriteLine("===== JWT PAYLOAD =====");
            //Console.WriteLine(jwtToken.Payload.SerializeToJson());

            return token;

        }
    }
}
