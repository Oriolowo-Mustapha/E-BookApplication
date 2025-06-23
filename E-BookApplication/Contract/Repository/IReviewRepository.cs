using E_BookApplication.Models.Entities;

namespace E_BookApplication.Contract.Repository
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetBookReviewsAsync(Guid bookId);
        Task<IEnumerable<Review>> GetUserReviewsAsync(string userId);
        Task<Review> GetUserBookReviewAsync(string userId, Guid bookId);
        Task<bool> HasUserReviewedBookAsync(string userId, Guid bookId);
        Task<double> GetBookAverageRatingAsync(Guid bookId);
        Task<bool> CanUserReviewBookAsync(string userId, Guid bookId);
    }
}
