using LyceumReviews.Models;
using LyceumReviews.Services;
using Microsoft.AspNetCore.Mvc;

namespace LyceumReviews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ILogger<ReviewsController> _logger;

        public ReviewsController(IReviewService reviewService, ILogger<ReviewsController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<PublicReviewDto>>> GetPublishedReviews()
        {
            try
            {
                var reviews = await _reviewService.GetPublishedReviewsAsync();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting published reviews");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Review>> CreateReview(CreateReviewDto reviewDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var review = await _reviewService.CreateReviewAsync(reviewDto);
                return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetReviewStats()
        {
            try
            {
                var averageRating = await _reviewService.GetAverageRatingAsync();
                var totalReviews = await _reviewService.GetTotalReviewsCountAsync();

                return Ok(new
                {
                    AverageRating = Math.Round(averageRating, 1),
                    TotalReviews = totalReviews,
                    RecommendationRate = 98 // Можна розрахувати на основі оцінок >= 4
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting review stats");
                return StatusCode(500, "Internal server error");
            }
        }

        // Admin endpoints
        [HttpGet("admin")]
        public async Task<ActionResult<List<Review>>> GetAllReviews()
        {
            try
            {
                var reviews = await _reviewService.GetAllReviewsAsync();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all reviews");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReviewById(string id)
        {
            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id);
                if (review == null)
                {
                    return NotFound();
                }
                return Ok(review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting review by id: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveReview(string id)
        {
            try
            {
                var result = await _reviewService.ApproveReviewAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving review: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}/publish")]
        public async Task<IActionResult> PublishReview(string id)
        {
            try
            {
                var result = await _reviewService.PublishReviewAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing review: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(string id)
        {
            try
            {
                var result = await _reviewService.DeleteReviewAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
