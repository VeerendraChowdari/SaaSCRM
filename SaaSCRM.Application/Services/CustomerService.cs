using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaSCRM.Application.Services
{
    public class CustomerService
    {
        public readonly ICustomerRepository _customerRepository;
        private readonly ICurrentUserService _currentUser;

        public CustomerService (ICustomerRepository customerRepository, ICurrentUserService currentUserService)
        {
            _customerRepository = customerRepository;
            _currentUser = currentUserService;
        }
        public async Task<string> AddAsync(Customer customer)
        {
            var res = await _customerRepository.GetByEmailAsync(customer.Email);

            if (res != null)
            {
                return "Customer already exists";
            }

            await _customerRepository.AddAsync(customer);

            return "Customer added successfully";
        }
        public async Task<Customer?> GetByEmail(string email) 
        {
           return await _customerRepository.GetByEmailAsync(email);
        }
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var tenantId = _currentUser.TenantId;

            return await _customerRepository.GetAllAsync(tenantId);
        }
        public async Task UpdateAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);
        }
        public async Task DeleteAsync(Customer customer)
        {
           await _customerRepository.DeleteAsync(customer.Id);
        }
        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }
    }
}
