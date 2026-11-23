
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.Application.Interfaces
{
    public interface IPendingRegistrationRepository
    {
        Task AddAsync(PendingRegistration registration);

        Task<PendingRegistration?> GetByOrderIdAsync(
            string orderId);

        Task SaveChangesAsync();
    }
}
