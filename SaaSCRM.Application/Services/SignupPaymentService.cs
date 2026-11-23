
using SaaSCRM.Application.DTOs;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Domain.Entities;

namespace SaaSCRM.Application.Services
{
    public class SignupPaymentService
    {
        private readonly ICashfreePaymentService _cashfree;
        private readonly IPendingRegistrationRepository _pendingRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly SubscriptionPlanService _plans;
        private readonly ITenantRegistrationRepository _tenantRegistrationRepository;
        private readonly IPaymentRepository _paymentRepository;

        public SignupPaymentService(
            ICashfreePaymentService cashfree,
            IPendingRegistrationRepository pendingRepository,
            IPasswordHasher passwordHasher,
            SubscriptionPlanService plans,
            ITenantRegistrationRepository tenantRegistrationRepository,
            IPaymentRepository paymentRepository)
        {
            _cashfree = cashfree;
            _pendingRepository = pendingRepository;
            _passwordHasher = passwordHasher;
            _plans = plans;
            _tenantRegistrationRepository = tenantRegistrationRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<CreatePaymentOrderResponse> CreateSignupOrderAsync(
            CreateSignupOrderRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CompanyName) ||
                string.IsNullOrWhiteSpace(request.CompanyEmail) ||
                string.IsNullOrWhiteSpace(request.PhoneNumber) ||
                string.IsNullOrWhiteSpace(request.AdminEmail) ||
                string.IsNullOrWhiteSpace(request.AdminPassword))
            {
                throw new ArgumentException(
                    "Please complete all required registration fields.");
            }

            if (request.AdminPassword.Length < 8)
            {
                throw new ArgumentException(
                    "Admin password must be at least 8 characters.");
            }

            var amount = _plans.GetPrice(request.PlanName);

            var companyName = request.CompanyName.Trim();
            var companyEmail = request.CompanyEmail.Trim();
            var phoneNumber = request.PhoneNumber.Trim();
            var adminEmail = request.AdminEmail.Trim();

            if (await _tenantRegistrationRepository.CompanyExistsAsync(companyName))
            {
                throw new ArgumentException("Company already exists.");
            }

            if (await _tenantRegistrationRepository.AdminEmailExistsAsync(adminEmail))
            {
                throw new ArgumentException("Admin email already exists.");
            }

            var userForHash = new User
            {
                Email = adminEmail
            };

            var passwordHash = _passwordHasher.HashPassword(
                userForHash,
                request.AdminPassword);

            var order = await _cashfree.CreateOrderAsync(
                new CreatePaymentOrderRequest
                {
                    CustomerName = companyName,
                    CustomerEmail = adminEmail,
                    CustomerPhone = phoneNumber,
                    Amount = amount
                });

            var payment = new Payment
            {
                OrderId = order.OrderId,
                PaymentSessionId = order.PaymentSessionId,
                Amount = amount,
                Currency = "INR",
                Status = "Pending",
                CustomerEmail = adminEmail,
                CreatedOn = DateTime.UtcNow
            };

            var pendingRegistration = new PendingRegistration
            {
                OrderId = order.OrderId,
                CompanyName = companyName,
                CompanyEmail = companyEmail,
                PhoneNumber = phoneNumber,
                Address = request.Address?.Trim() ?? string.Empty,
                AdminEmail = adminEmail,
                AdminPasswordHash = passwordHash,
                PlanName = request.PlanName.Trim(),
                Status = "Pending",
                CreatedOn = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);
            await _pendingRepository.AddAsync(pendingRegistration);

            // Both repositories must use the same scoped DbContext.
            await _paymentRepository.SaveChangesAsync();

            return order;
        }

        public async Task<string> ActivateSignupAsync(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentException("Order ID is required.");
            }

            var pendingRegistration =
                await _pendingRepository.GetByOrderIdAsync(orderId);

            if (pendingRegistration == null)
            {
                throw new ArgumentException(
                    "Signup registration was not found.");
            }

            if (pendingRegistration.CompletedOn != null ||
                string.Equals(
                    pendingRegistration.Status,
                    "Completed",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Registration already completed.";
            }

            // Verify the order directly with Cashfree.
            // Never trust the browser redirect as proof of payment.
            var paymentStatus =
                await _cashfree.GetOrderStatusAsync(orderId);

            if (!string.Equals(
                    paymentStatus.OrderId,
                    pendingRegistration.OrderId,
                    StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    "Payment order ID verification failed.");
            }

            var expectedAmount =
                _plans.GetPrice(pendingRegistration.PlanName);

            if (paymentStatus.Amount != expectedAmount ||
                !string.Equals(
                    paymentStatus.Currency,
                    "INR",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Payment amount or currency verification failed.");
            }

            if (!string.Equals(
                    paymentStatus.Status,
                    "PAID",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "Payment is not completed yet.";
            }

            var payment =
                await _paymentRepository.GetByOrderIdAsync(orderId);

            if (payment == null)
            {
                throw new InvalidOperationException(
                    "Payment record was not found for this signup.");
            }

            if (payment.Amount != expectedAmount ||
                !string.Equals(
                    payment.Currency,
                    "INR",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Stored payment amount or currency verification failed.");
            }

            if (await _tenantRegistrationRepository.CompanyExistsAsync(
                    pendingRegistration.CompanyName))
            {
                throw new ArgumentException(
                    "Company already exists. Please contact support.");
            }

            if (await _tenantRegistrationRepository.AdminEmailExistsAsync(
                    pendingRegistration.AdminEmail))
            {
                throw new ArgumentException(
                    "Admin email already exists. Please contact support.");
            }

            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                CompanyName = pendingRegistration.CompanyName,
                Email = pendingRegistration.CompanyEmail,
                PhoneNumber = pendingRegistration.PhoneNumber,
                Address = pendingRegistration.Address,
                Slug = await GenerateUniqueSlugAsync(pendingRegistration.CompanyName),
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Email = pendingRegistration.AdminEmail,
                PasswordHash = pendingRegistration.AdminPasswordHash,
                UserRole = "Admin",
                TenantId = tenant.Id,
                FirstName = "Admin",
                LastName = "User",
                Phoneno = pendingRegistration.PhoneNumber
            };

            // This repository commits tenant, admin, and registration status
            // in its own transaction.

            await _tenantRegistrationRepository.CreateTenantAndAdminAsync(
                tenant,
                adminUser,
                pendingRegistration,
                payment);

            return "Payment verified. Tenant and Admin created successfully.";

        }

        private static string GenerateSlug(string companyName)
        {
            var slug = companyName.Trim().ToLowerInvariant();

            slug = System.Text.RegularExpressions.Regex
                .Replace(slug, @"[^a-z0-9]+", "-");

            return slug.Trim('-');
        }
        private async Task<string> GenerateUniqueSlugAsync(string companyName)
        {
            var baseSlug = GenerateSlug(companyName);
            var slug = baseSlug;
            var counter = 2;

            while (await _tenantRegistrationRepository.SlugExistsAsync(slug))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }
    }
}
