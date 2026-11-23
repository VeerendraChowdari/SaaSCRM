
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.Application.Interfaces
{
    public interface ITenantRegistrationRepository
    {
        Task<bool> CompanyExistsAsync(string companyName);

        Task<bool> AdminEmailExistsAsync(string adminEmail);

        Task<bool> SlugExistsAsync(string slug);

        Task CreateTenantAndAdminAsync(
            Tenant tenant,
            User adminUser,
            PendingRegistration pendingRegistration,
            Payment payment);
    }
}
