using SaaSCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSCRM.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByEmailAsync(string email);
    }
}
