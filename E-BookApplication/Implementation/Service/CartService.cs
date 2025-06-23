using AutoMapper;
using E_BookApplication.DTOs;
using E_BookApplication.Interface.Repository;
using E_BookApplication.Interface.Service;
using E_BookApplication.Models.Entities;


namespace E_BookApplication.Implementation.Service
{
	public class CartService : ICartService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CartService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<CartItemDTO>> GetUserCartAsync(string userId)
		{
			var cartItems = await _unitOfWork.Cart.GetUserCartAsync(userId);
			return _mapper.Map<IEnumerable<CartItemDTO>>(cartItems);
		}

		public async Task<CartItemDTO> AddToCartAsync(string userId, AddToCartDTO addToCartDto)
		{
			var existingItem = await _unitOfWork.Cart.GetCartItemAsync(userId, addToCartDto.BookId);

			if (existingItem != null)
			{
				existingItem.Quantity += addToCartDto.Quantity;
				_unitOfWork.Cart.Update(existingItem);
			}
			else
			{
				var cartItem = new CartItem
				{
					UserId = userId,
					BookId = addToCartDto.BookId,
					Quantity = addToCartDto.Quantity,
					AddedAt = DateTime.UtcNow
				};
				await _unitOfWork.Cart.AddAsync(cartItem);
				existingItem = cartItem;
			}

			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<CartItemDTO>(existingItem);
		}

		public async Task<CartItemDTO> UpdateCartItemAsync(string userId, UpdateCartItemDTO updateCartDto)
		{
			var cartItem = await _unitOfWork.Cart.FirstOrDefaultAsync(c => c.Id == updateCartDto.CartItemId && c.UserId == userId);
			if (cartItem == null) return null;

			cartItem.Quantity = updateCartDto.Quantity;
			_unitOfWork.Cart.Update(cartItem);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<CartItemDTO>(cartItem);
		}

		public async Task<bool> RemoveFromCartAsync(string userId, Guid cartItemId)
		{
			var cartItem = await _unitOfWork.Cart.FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);
			if (cartItem == null) return false;

			_unitOfWork.Cart.Remove(cartItem);
			await _unitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<bool> ClearCartAsync(string userId)
		{
			await _unitOfWork.Cart.ClearUserCartAsync(userId);
			await _unitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<decimal> GetCartTotalAsync(string userId)
		{
			return await _unitOfWork.Cart.GetCartTotalAsync(userId);
		}

		public async Task<int> GetCartItemCountAsync(string userId)
		{
			return await _unitOfWork.Cart.GetCartItemCountAsync(userId);
		}
	}
}

