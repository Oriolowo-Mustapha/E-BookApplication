using E_BookApplication.Models.Entities;

namespace E_BookApplication.Contract.Repository
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetBookReviewsAsync(Guid bookId);
        Task<IEnumerable<Review>> GetUserReviewsAsync(Guid userId);
        Task<Review> GetUserBookReviewAsync(Guid userId, Guid bookId);
        Task<bool> HasUserReviewedBookAsync(Guid userId, Guid bookId);
        Task<double> GetBookAverageRatingAsync(Guid bookId);
        Task<bool> CanUserReviewBookAsync(Guid userId, Guid bookId);
    }
}
