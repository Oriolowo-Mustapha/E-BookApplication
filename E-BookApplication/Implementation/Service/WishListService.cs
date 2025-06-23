using AutoMapper;
using E_BookApplication.DTOs;
using E_BookApplication.Interface.Repository;
using E_BookApplication.Interface.Service;
using E_BookApplication.Models.Entities;

namespace E_BookApplication.Implementation.Service
{
	public class WishlistService : IWishlistService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public WishlistService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<WishlistDTO>> GetUserWishlistAsync(string userId)
		{
			var wishlistItems = await _unitOfWork.Wishlist.GetUserWishlistAsync(userId);
			return _mapper.Map<IEnumerable<WishlistDTO>>(wishlistItems);
		}

		public async Task<WishlistDTO> AddToWishlistAsync(string userId, AddToWishlistDTO addToWishlistDto)
		{
			var existingItem = await _unitOfWork.Wishlist.FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == addToWishlistDto.BookId);
			if (existingItem != null)
				throw new InvalidOperationException("Book is already in wishlist");

			var wishlistItem = new Wishlist
			{
				UserId = userId,
				BookId = addToWishlistDto.BookId,
				AddedAt = DateTime.UtcNow
			};

			await _unitOfWork.Wishlist.AddAsync(wishlistItem);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<WishlistDTO>(wishlistItem);
		}

		public async Task<bool> RemoveFromWishlistAsync(string userId, Guid bookId)
		{
			var wishlistItem = await _unitOfWork.Wishlist.FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId);
			if (wishlistItem == null) return false;

			_unitOfWork.Wishlist.Remove(wishlistItem);
			await _unitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<bool> IsBookInWishlistAsync(string userId, Guid bookId)
		{
			var wishlistItem = await _unitOfWork.Wishlist.FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId);
			return wishlistItem != null;
		}
	}
}
