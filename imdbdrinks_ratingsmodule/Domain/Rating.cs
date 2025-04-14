using System;
using imdbdrinks_ratingsmodule.Constants;

namespace imdbdrinks_ratingsmodule.Domain
{
    /// <summary>
    /// Represents a user's rating for a product.
    /// </summary>
    public class Rating
    {
        /// <summary>
        /// Gets or sets the unique identifier for the rating.
        /// </summary>
        public int RatingId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated product.
        /// </summary>
        public int ProductId { get; set; } // Reference to the product (for future integration)

        /// <summary>
        /// Gets or sets the identifier of the user who submitted the rating.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the rating value, ranging from 1 to 5 stars.
        /// </summary>
        public double RatingValue { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the rating was submitted.
        /// </summary>
        public DateTime RatingDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the rating is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Validates that the rating value is within the acceptable range (1 to 5).
        /// </summary>
        /// <returns>True if the rating value is valid; otherwise, false.</returns>
        public bool IsValid() => RatingValue >= RatingDomainConstants.MinRatingValue && RatingValue <= RatingDomainConstants.MaxRatingValue;
    }
}
