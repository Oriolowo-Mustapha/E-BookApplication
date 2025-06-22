using E_BookApplication.DTOs;

namespace E_BookApplication.Contract.Service
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<PaymentMethodDTO>> GetAllPaymentMethodsAsync();
        Task<IEnumerable<PaymentMethodDTO>> GetActivePaymentMethodsAsync();
        Task<PaymentMethodDTO> GetPaymentMethodByIdAsync(int paymentMethodId);
        Task<PaymentMethodDTO> CreatePaymentMethodAsync(CreatePaymentMethodDTO createPaymentMethodDto);
        Task<PaymentMethodDTO> UpdatePaymentMethodAsync(int paymentMethodId, CreatePaymentMethodDTO updatePaymentMethodDto);
        Task<bool> DeletePaymentMethodAsync(int paymentMethodId);
    }
}
