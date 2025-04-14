namespace imdbdrinks_ratingsmodule.Domain
{
    using System;

    /// <summary>
    /// Represents a user's review for a rating.
    /// </summary>
    public class Review
    {
        private const int MaxContentLength = 500;

        /// <summary>
        /// Gets or sets the unique identifier for the review.
        /// </summary>
        public int ReviewId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated rating.
        /// </summary>
        public int RatingId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who submitted the review.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the content of the review.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the review was created.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the review is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Validates that the review content is not empty and does not exceed the maximum length.
        /// </summary>
        /// <returns>True if the review is valid; otherwise, false.</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(this.Content) && this.Content.Length <= MaxContentLength;
        }

        /// <summary>
        /// Activates the review, setting its creation date to the current time.
        /// </summary>
        public void Activate()
        {
            this.IsActive = true;
            this.CreationDate = DateTime.Now;
        }
    }
}
