using SaaSCRM.Domain.Entities;

namespace SaaSCRM.Application.Interfaces
{
    public interface ILeadRepository
    {
        Task AddLeadAsync(Lead lead);
        Task UpdateLeadAsync(Lead lead);
        Task DeleteLeadAsync(Lead lead);

        Task<Lead?> GetLeadByIdAsync(Guid id);
        Task<Lead?> GetLeadByNameAsync(string name);
        Task<Lead?> GetLeadByEmailAsync(string email);

        Task<IEnumerable<Lead>> GetAllLeadsAsync(Guid tenantId);
    }
}