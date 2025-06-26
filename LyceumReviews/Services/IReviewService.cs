using LyceumReviews.Models;

namespace LyceumReviews.Services
{
    public interface IReviewService
    {
        Task<List<PublicReviewDto>> GetPublishedReviewsAsync();
        Task<Review> CreateReviewAsync(CreateReviewDto reviewDto);
        Task<List<Review>> GetAllReviewsAsync();
        Task<Review?> GetReviewByIdAsync(string id);
        Task<bool> ApproveReviewAsync(string id);
        Task<bool> PublishReviewAsync(string id);
        Task<bool> DeleteReviewAsync(string id);
        Task<double> GetAverageRatingAsync();
        Task<int> GetTotalReviewsCountAsync();
    }
}