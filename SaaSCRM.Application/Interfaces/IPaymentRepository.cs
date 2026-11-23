
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);

        Task SaveChangesAsync();

        Task<Payment?> GetByOrderIdAsync(string orderId);
    }
}
