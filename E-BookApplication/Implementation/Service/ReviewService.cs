using AutoMapper;
using E_BookApplication.Contract.Repository;
using E_BookApplication.Contract.Service;
using E_BookApplication.DTOs;
using E_BookApplication.Models.Entities;

namespace E_BookApplication.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReviewDTO> CreateReviewAsync(string userId, CreateReviewDTO createReviewDto)
        {
            // Check if user has purchased the book
            var canReview = await CanUserReviewBookAsync(userId, createReviewDto.BookId);
            if (!canReview)
                throw new InvalidOperationException("You can only review books you have purchased");

            // Check if user has already reviewed this book
            var existingReview = await GetUserBookReviewAsync(userId, createReviewDto.BookId);
            if (existingReview != null)
                throw new InvalidOperationException("You have already reviewed this book");

            var review = new Review
            {
                UserId = userId,
                BookId = createReviewDto.BookId,
                Rating = createReviewDto.Rating,
                Comment = createReviewDto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<ReviewDTO> UpdateReviewAsync(Guid reviewId, CreateReviewDTO updateReviewDto, string userId)
        {
            var review = await _unitOfWork.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);
            if (review == null) return null;

            review.Rating = updateReviewDto.Rating;
            review.Comment = updateReviewDto.Comment;
            review.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Reviews.Update(review);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<bool> DeleteReviewAsync(Guid reviewId, string userId)
        {
            var review = await _unitOfWork.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);
            if (review == null) return false;

            _unitOfWork.Reviews.Remove(review);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ReviewDTO>> GetBookReviewsAsync(Guid bookId)
        {
            var reviews = await _unitOfWork.Reviews.GetBookReviewsAsync(bookId);
            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        }

        public async Task<IEnumerable<ReviewDTO>> GetUserReviewsAsync(string userId)
        {
            var reviews = await _unitOfWork.Reviews.GetUserReviewsAsync(userId);
            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        }

        public async Task<ReviewDTO> GetUserBookReviewAsync(string userId, Guid bookId)
        {
            var review = await _unitOfWork.Reviews.GetUserBookReviewAsync(userId, bookId);
            return review != null ? _mapper.Map<ReviewDTO>(review) : null;
        }

        public async Task<bool> CanUserReviewBookAsync(string userId, Guid bookId)
        {
            return await _unitOfWork.Reviews.CanUserReviewBookAsync(userId, bookId);
        }

        public async Task<double> GetBookAverageRatingAsync(Guid bookId)
        {
            return await _unitOfWork.Reviews.GetBookAverageRatingAsync(bookId);
        }
    }
}
