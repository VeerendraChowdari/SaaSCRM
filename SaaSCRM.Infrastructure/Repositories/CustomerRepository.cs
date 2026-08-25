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
    public class CustomerRepository : ICustomerRepository
    {
        public readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context)
        {
          _context = context;  
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer != null) 
            {
                _context.Customers.Remove(existingCustomer);
               await _context.SaveChangesAsync();
            }
            return;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(Guid tenantId)
        {
           return await _context.Customers.Where(c => c.TenantId == tenantId).ToListAsync();
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public Task<Customer?> GetByIdAsync(Guid id)
        {
           return _context.Customers.FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task UpdateAsync(Customer customer)
        {
            var existingCustomer =await _context.Customers.FindAsync(customer.Id);
            if(existingCustomer != null)
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
