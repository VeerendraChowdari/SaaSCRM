
using Microsoft.EntityFrameworkCore;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.Infrastructure.Persistence
{
    public class PendingRegistrationRepository
        : IPendingRegistrationRepository
    {
        private readonly ApplicationDbContext _context;

        public PendingRegistrationRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            PendingRegistration registration)
        {
            await _context.PendingRegistrations.AddAsync(
                registration);
        }

        public async Task<PendingRegistration?> GetByOrderIdAsync(
            string orderId)
        {
            return await _context.PendingRegistrations
                .FirstOrDefaultAsync(
                    x => x.OrderId == orderId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
