using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LyceumReviews.Models
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("parentName")]
        [Required]
        [StringLength(100)]
        public string ParentName { get; set; } = string.Empty;

        [BsonElement("studentName")]
        [Required]
        [StringLength(100)]
        public string StudentName { get; set; } = string.Empty;

        [BsonElement("email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BsonElement("rating")]
        [Range(1, 5)]
        public int Rating { get; set; }

        [BsonElement("reviewText")]
        [Required]
        [StringLength(1000)]
        public string ReviewText { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isApproved")]
        public bool IsApproved { get; set; } = false;

        [BsonElement("isPublished")]
        public bool IsPublished { get; set; } = false;
    }
}