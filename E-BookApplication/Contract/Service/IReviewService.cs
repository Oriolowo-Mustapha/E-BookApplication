using E_BookApplication.DTOs;

namespace E_BookApplication.Contract.Service
{
    public interface IReviewService
    {
        Task<ReviewDTO> CreateReviewAsync(string userId, CreateReviewDTO createReviewDto);
        Task<ReviewDTO> UpdateReviewAsync(Guid reviewId, CreateReviewDTO updateReviewDto, string userId);
        Task<bool> DeleteReviewAsync(Guid reviewId, string userId);
        Task<IEnumerable<ReviewDTO>> GetBookReviewsAsync(Guid bookId);
        Task<IEnumerable<ReviewDTO>> GetUserReviewsAsync(string userId);
        Task<ReviewDTO> GetUserBookReviewAsync(string userId, Guid bookId);
        Task<bool> CanUserReviewBookAsync(string userId, Guid bookId);
        Task<double> GetBookAverageRatingAsync(Guid bookId);
    }
}
