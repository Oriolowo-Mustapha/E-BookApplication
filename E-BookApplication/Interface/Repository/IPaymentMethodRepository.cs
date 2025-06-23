using E_BookApplication.Interface.Repository;
using E_BookApplication.Models.Entities;

namespace E_BookApplication.Interface.Repository
{
    public interface IPaymentMethodRepository : IGenericRepository<PaymentMethod>
    {
        Task<IEnumerable<PaymentMethod>> GetActivePaymentMethodsAsync();
        Task<PaymentMethod> GetByBankCodeAsync(string bankCode);
    }
}
