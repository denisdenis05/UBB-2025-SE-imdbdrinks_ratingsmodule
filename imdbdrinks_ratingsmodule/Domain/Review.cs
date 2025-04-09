namespace imdbdrinks_ratingsmodule.Domain
{
    using System;

    public class Review
    {
        public int ReviewId { get; set; }

        public int RatingId { get; set; } // The rating this review belongs to

        public int UserId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; }

        public bool IsActive { get; set; }

        // Validate that the review content is not empty and no longer than 500 characters.
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(this.Content) && this.Content.Length <= 500;
        }
    }
}
