using AutoMapper;
using E_BookApplication.Contract.Repository;
using E_BookApplication.Contract.Service;
using E_BookApplication.DTOs;
using E_BookApplication.Models.Entities;
using E_BookApplication.Models.Entities.Enum;
using Microsoft.AspNetCore.Identity;


namespace EBookStore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _userManager = userManager;
        }


        public async Task<OrderDTO> CreateOrderAsync(Guid userId, CreateOrderDTO createOrderDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Get cart items
                var cartItems = await _unitOfWork.Cart.GetUserCartAsync(userId);
                if (!cartItems.Any())
                    throw new InvalidOperationException("Cart is empty");

                // Calculate totals
                var subtotal = cartItems.Sum(item => item.Book.Price * item.Quantity);
                var discount = 0m;

                // Apply coupon if provided
                if (!string.IsNullOrEmpty(createOrderDto.CouponCode))
                {
                    var coupon = await _unitOfWork.Coupons.GetByCodeAsync(createOrderDto.CouponCode);
                    if (coupon != null && await _unitOfWork.Coupons.ValidateCouponAsync(createOrderDto.CouponCode))
                    {
                        discount = coupon.DiscountType == "Percentage"
                            ? subtotal * (coupon.DiscountValue / 100)
                            : coupon.DiscountValue;
                        await _unitOfWork.Coupons.IncrementUsageAsync(coupon.Id);
                    }
                }

                var total = subtotal - discount;

                // Create order
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.pending,
                    SubTotal = subtotal,
                    DiscountAmount = discount,
                    TotalAmount = total,
                    PaymentMethodId = createOrderDto.PaymentMethodId,
                    ShippingAddress = createOrderDto.ShippingAddress,
                    CouponId = !string.IsNullOrEmpty(createOrderDto.CouponCode)
                        ? (await _unitOfWork.Coupons.GetByCodeAsync(createOrderDto.CouponCode))?.Id
                        : null
                };

                await _unitOfWork.Orders.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

                // Create order items
                foreach (var cartItem in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        BookId = cartItem.BookId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = cartItem.Book.Price,
                        TotalPrice = cartItem.Book.Price * cartItem.Quantity
                    };
                    await _unitOfWork.OrderItem.AddAsync(orderItem);
                }

                // Clear cart
                await _unitOfWork.Cart.ClearUserCartAsync(userId);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                var orderDto = _mapper.Map<OrderDTO>(order);


                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user != null)
                {
                    await _emailService.SendOrderConfirmationEmailAsync(user.Email, orderDto);
                }

                return orderDto;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<OrderDTO> GetOrderByIdAsync(Guid orderId, Guid userId)
        {
            var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(orderId);
            if (order == null || order.UserId != userId) return null;

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetUserOrdersAsync(Guid userId)
        {
            var orders = await _unitOfWork.Orders.GetUserOrdersAsync(userId);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<IEnumerable<OrderDTO>> GetVendorOrdersAsync(string vendorId)
        {
            var orders = await _unitOfWork.Orders.GetVendorOrdersAsync(vendorId);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> UpdateOrderStatusAsync(Guid orderId, string status)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null) return null;

            // Fixed: Parse string to enum or accept OrderStatus parameter
            if (Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
            {
                order.Status = orderStatus;
                order.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Orders.Update(order);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<OrderDTO>(order);
            }

            return null; 
        }

        public async Task<bool> CancelOrderAsync(Guid orderId, Guid userId)
        {
            var order = await _unitOfWork.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            if (order == null || order.Status != OrderStatus.pending) 
                return false;

            order.Status = OrderStatus.cancelled;
            order.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<decimal> GetTotalSalesAsync(string vendorId = null)
        {
            return await _unitOfWork.Orders.GetTotalSalesAsync(vendorId);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _unitOfWork.Orders.GetOrdersByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }
    }
} 
