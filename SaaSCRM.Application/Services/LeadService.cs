using SaaSCRM.Application.DTOs;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.Application.Services
{
    public class LeadService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ICurrentUserService _currentUser;

        public LeadService(
            ILeadRepository leadRepository,
            ICurrentUserService currentUserService)
        {
            _leadRepository = leadRepository;
            _currentUser = currentUserService;
        }

        public async Task<string> AddLeadAsync(CreateLeadRequest request)
        {
            var existingLead = await _leadRepository
                .GetLeadByEmailAsync(request.Email);

            if (existingLead != null)
            {
                return "Lead already exists";
            }

            var lead = new Lead
            {
                Name = request.Name,
                CompanyName = request.CompanyName,
                Email = request.Email,
                Phone = request.Phone,
                Source = request.Source,
                Status = request.Status,
                AssignedToUserId = request.AssignedToUserId,
                TenantId = _currentUser.TenantId
            };

            await _leadRepository.AddLeadAsync(lead);

            return "Lead added successfully";
        }

        public async Task<IEnumerable<Lead>> GetAllLeadsAsync()
        {
            return await _leadRepository
                .GetAllLeadsAsync(_currentUser.TenantId);
        }

        public async Task<Lead?> GetLeadByIdAsync(Guid id)
        {
            return await _leadRepository.GetLeadByIdAsync(id);
        }

        public async Task<Lead?> GetLeadByEmailAsync(string email)
        {
            return await _leadRepository.GetLeadByEmailAsync(email);
        }

        public async Task<Lead?> GetLeadByNameAsync(string name)
        {
            return await _leadRepository.GetLeadByNameAsync(name);
        }

        public async Task UpdateLeadAsync(Guid id, UpdateLeadRequest request)
        {
            var existingLead = await _leadRepository.GetLeadByIdAsync(id);

            if (existingLead == null)
                return;

            existingLead.Name = request.Name;
            existingLead.CompanyName = request.CompanyName;
            existingLead.Email = request.Email;
            existingLead.Phone = request.Phone;
            existingLead.Source = request.Source;
            existingLead.Status = request.Status;
            existingLead.AssignedToUserId = request.AssignedToUserId;

            await _leadRepository.UpdateLeadAsync(existingLead);
        }

        public async Task<bool> DeleteLeadAsync(Guid id)
        {
            var existingLead = await _leadRepository.GetLeadByIdAsync(id);

            if (existingLead == null)
                return false;

            await _leadRepository.DeleteLeadAsync(existingLead);

            return true;
        }
    }
}