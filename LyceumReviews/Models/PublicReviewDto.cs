namespace LyceumReviews.Models
{
    public class PublicReviewDto
    {
        public string Id { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string ReviewText { get; set; } = string.Empty;
        public string? Grade { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
