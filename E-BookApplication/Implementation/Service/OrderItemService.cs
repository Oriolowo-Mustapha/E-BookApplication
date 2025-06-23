using E_BookApplication.Contract.Repository;
using E_BookApplication.Contract.Service;
using E_BookApplication.Models.Entities;

namespace E_BookApplication.Service
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderItem> CreateOrderItemAsync(Guid orderId, Guid bookId, int quantity, decimal unitPrice)
        {

            var existingItem = await _unitOfWork.OrderItem.GetByOrderAndBookIdAsync(orderId, bookId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
                _unitOfWork.OrderItem.Update(existingItem);
                await _unitOfWork.SaveChangesAsync();
                return existingItem;
            }

            var orderItem = new OrderItem
            {
                OrderId = orderId,
                BookId = bookId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                TotalPrice = quantity * unitPrice
            };

       
            await _unitOfWork.OrderItem.AddAsync(orderItem);
            await _unitOfWork.SaveChangesAsync();
            return orderItem;
        }

        public async Task<OrderItem> UpdateQuantityAsync(Guid orderItemId, int newQuantity)
        {
          
            var orderItem = await _unitOfWork.OrderItem.GetByIdAsync(orderItemId);
            if (orderItem == null)
                throw new ArgumentException("Order item not found");

            orderItem.Quantity = newQuantity;
            orderItem.TotalPrice = newQuantity * orderItem.UnitPrice;

           
            _unitOfWork.OrderItem.Update(orderItem);
            await _unitOfWork.SaveChangesAsync();
            return orderItem;
        }

        public async Task<bool> RemoveOrderItemAsync(Guid orderItemId)
        {
           
            var orderItem = await _unitOfWork.OrderItem.FirstOrDefaultAsync(oi => oi.Id == orderItemId);
            if (orderItem == null)
                return false;

            _unitOfWork.OrderItem.Remove(orderItem);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }



        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(Guid orderId)
        {
            return await _unitOfWork.OrderItem.GetByOrderIdAsync(orderId);
        }

        public async Task<decimal> CalculateOrderTotalAsync(Guid orderId)
        {
            var orderItems = await _unitOfWork.OrderItem.GetByOrderIdAsync(orderId);
            return orderItems.Sum(oi => oi.TotalPrice);
        }
    }
}
