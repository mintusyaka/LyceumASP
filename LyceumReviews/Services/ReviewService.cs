using LyceumReviews.Models;
using MongoDB.Driver;

namespace LyceumReviews.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMongoCollection<Review> _reviewsCollection;

        public ReviewService(IMongoDatabase database)
        {
            _reviewsCollection = database.GetCollection<Review>("reviews");
        }

        public async Task<List<PublicReviewDto>> GetPublishedReviewsAsync()
        {
            var filter = Builders<Review>.Filter.And(
                Builders<Review>.Filter.Eq(r => r.IsApproved, true),
                Builders<Review>.Filter.Eq(r => r.IsPublished, true)
            );

            var reviews = await _reviewsCollection
                .Find(filter)
                .SortByDescending(r => r.CreatedAt)
                .ToListAsync();

            return reviews.Select(r => new PublicReviewDto
            {
                Id = r.Id!,
                ParentName = r.ParentName,
                StudentName = r.StudentName,
                Rating = r.Rating,
                ReviewText = r.ReviewText,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<Review> CreateReviewAsync(CreateReviewDto reviewDto)
        {
            var review = new Review
            {
                ParentName = reviewDto.ParentName,
                StudentName = reviewDto.StudentName,
                Email = reviewDto.Email,
                Rating = reviewDto.Rating,
                ReviewText = reviewDto.ReviewText,
                CreatedAt = DateTime.UtcNow,
                IsApproved = false,
                IsPublished = false
            };

            await _reviewsCollection.InsertOneAsync(review);
            return review;
        }

        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _reviewsCollection
                .Find(_ => true)
                .SortByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(string id)
        {
            return await _reviewsCollection
                .Find(r => r.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ApproveReviewAsync(string id)
        {
            var filter = Builders<Review>.Filter.Eq(r => r.Id, id);
            var update = Builders<Review>.Update.Set(r => r.IsApproved, true);

            var result = await _reviewsCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> PublishReviewAsync(string id)
        {
            var filter = Builders<Review>.Filter.Eq(r => r.Id, id);
            var update = Builders<Review>.Update.Set(r => r.IsPublished, true);

            var result = await _reviewsCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteReviewAsync(string id)
        {
            var result = await _reviewsCollection.DeleteOneAsync(r => r.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<double> GetAverageRatingAsync()
        {
            var filter = Builders<Review>.Filter.And(
                Builders<Review>.Filter.Eq(r => r.IsApproved, true),
                Builders<Review>.Filter.Eq(r => r.IsPublished, true)
            );

            var reviews = await _reviewsCollection.Find(filter).ToListAsync();
            return reviews.Any() ? reviews.Average(r => r.Rating) : 0;
        }

        public async Task<int> GetTotalReviewsCountAsync()
        {
            var filter = Builders<Review>.Filter.And(
                Builders<Review>.Filter.Eq(r => r.IsApproved, true),
                Builders<Review>.Filter.Eq(r => r.IsPublished, true)
            );

            return (int)await _reviewsCollection.CountDocumentsAsync(filter);
        }
    }
}