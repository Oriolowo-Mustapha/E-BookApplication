using E_BookApplication.Models.Entities;

namespace E_BookApplication.Contract.Repository
{
    public interface IPaymentMethodRepository : IGenericRepository<PaymentMethod>
    {
        Task<IEnumerable<PaymentMethod>> GetActivePaymentMethodsAsync();
        Task<PaymentMethod> GetByBankCodeAsync(string bankCode);
    }
}
