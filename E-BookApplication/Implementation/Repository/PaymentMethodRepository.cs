using E_BookApplication.Contract.Repository;
using E_BookApplication.Data;
using E_BookApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_BookApplication.Repository
{
    public class PaymentMethodRepository : GenericRepository<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(EBookDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PaymentMethod>> GetActivePaymentMethodsAsync()
        {
            return await _dbSet.Where(p => p.IsActive).ToListAsync();
        }

        public async Task<PaymentMethod> GetByBankCodeAsync(string bankCode)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.BankCode == bankCode);
        }
    }
}
