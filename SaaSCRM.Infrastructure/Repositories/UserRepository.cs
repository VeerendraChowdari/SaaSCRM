using Microsoft.EntityFrameworkCore;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;
using SaaSCRM.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSCRM.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly ApplicationDbContext _Context;
        public UserRepository(ApplicationDbContext context)
        {
            _Context = context;
        }

        public async Task AddAsync(User user)
        {
            await _Context.Users.AddAsync(user);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var ExistingUser = await _Context.Users.FindAsync(id);
            if (ExistingUser != null)
            {
                _Context.Users.Remove(ExistingUser);
                await _Context.SaveChangesAsync();
            }
            else
                return;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _Context.Users.ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _Context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _Context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            var ExistingUser = await _Context.Users.FindAsync(user.Id);
            if (ExistingUser == null)
                return;
            else
            {
                _Context.Users.Update(user);
                await _Context.SaveChangesAsync();
            }
        }
    }
}
