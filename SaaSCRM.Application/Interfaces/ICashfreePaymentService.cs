
using SaaSCRM.Application.DTOs;

namespace SaaSCRM.Application.Interfaces
{
    public interface ICashfreePaymentService
    {
        Task<CreatePaymentOrderResponse> CreateOrderAsync(
            CreatePaymentOrderRequest request);

        Task<PaymentStatusResponse> GetOrderStatusAsync(
            string orderId);
    }
}
