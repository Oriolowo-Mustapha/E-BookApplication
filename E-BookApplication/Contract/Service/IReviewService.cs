using E_BookApplication.DTOs;

namespace E_BookApplication.Contract.Service
{
    public interface IReviewService
    {
        Task<ReviewDTO> CreateReviewAsync(Guid userId, CreateReviewDTO createReviewDto);
        Task<ReviewDTO> UpdateReviewAsync(Guid reviewId, CreateReviewDTO updateReviewDto, Guid userId);
        Task<bool> DeleteReviewAsync(Guid reviewId, Guid userId);
        Task<IEnumerable<ReviewDTO>> GetBookReviewsAsync(Guid bookId);
        Task<IEnumerable<ReviewDTO>> GetUserReviewsAsync(Guid userId);
        Task<ReviewDTO> GetUserBookReviewAsync(Guid userId, Guid bookId);
        Task<bool> CanUserReviewBookAsync(Guid userId, Guid bookId);
        Task<double> GetBookAverageRatingAsync(Guid bookId);
    }
}
