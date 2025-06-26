using System.ComponentModel.DataAnnotations;

namespace LyceumReviews.Models
{
    public class CreateReviewDto
    {
        [Required]
        [StringLength(100)]
        public string ParentName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string StudentName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string ReviewText { get; set; } = string.Empty;

    }
}