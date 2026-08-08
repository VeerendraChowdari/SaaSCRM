using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.Application.Services
{
    public class UserService
    {
        public readonly IUserRepository _UserRepository;

        public UserService(IUserRepository userRepository)
        {
            _UserRepository = userRepository;
        }

        public async Task<string> AddAsync(User user)
        {
            var res = await _UserRepository.GetByEmailAsync(user.Email);

            if (res != null)
            {
                return "User already exists with this email.";
            }

            await _UserRepository.AddAsync(user);

            return "User created successfully.";
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _UserRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _UserRepository.GetAllAsync();
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
    }
}