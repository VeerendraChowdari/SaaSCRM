using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SaaSCRM.Application.Interfaces;

namespace SaaSCRM.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId =>
            Guid.Parse(
                _httpContextAccessor.HttpContext!
                    .User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        public Guid TenantId =>
            Guid.Parse(
                _httpContextAccessor.HttpContext!
                    .User.FindFirst("tenantId")!.Value);

        public string? Email =>
            _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.Email)?.Value;

        public string? Role =>
            _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.Role)?.Value;
    }
}