using SaaSCRM.Domain.Entities;

namespace SaaSCRM.Application.Interfaces;
public interface ITenantRepository
{
    Task AddAsync(Tenant tenant);
    Task UpdateAsync(Tenant tenant);
    Task DeleteAsync(Guid id);
    Task<Tenant?> GetByIdAsync(Guid id);
    Task<IEnumerable<Tenant>> GetAllAsync();
    Task<bool> ExistsAsync(string companyName);
    Task<Tenant?> GetByEmailAsync(string email);
    Task<Tenant?> GetByCompanyNameAsync(string companyName);
}