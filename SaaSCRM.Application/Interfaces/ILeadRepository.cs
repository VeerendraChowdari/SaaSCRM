using SaaSCRM.Domain.Entities;

namespace SaaSCRM.Application.Interfaces
{
    public interface ILeadRepository
    {
        Task AddLeadAsync(Lead lead);
        Task UpdateLeadAsync(Lead lead);
        Task DeleteLeadAsync(Lead lead);

        Task<Lead?> GetLeadByIdAsync(Guid id, Guid tenantId);
        Task<Lead?> GetLeadByNameAsync(string name, Guid tenantId);
        Task<Lead?> GetLeadByEmailAsync(string email, Guid tenantId);

        Task<IEnumerable<Lead>> GetAllLeadsAsync(Guid tenantId);
    }
}