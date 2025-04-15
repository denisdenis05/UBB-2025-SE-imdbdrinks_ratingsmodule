using imdbdrinks_ratingsmodule.Domain;
using System;
using System.Collections.Generic;

namespace imdbdrinks_ratingsmodule.Services
{
    public interface IReviewService
    {
        /// <summary>
        /// Retrieves all reviews associated with a specific rating.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>A collection of <see cref="Review"/> instances for the rating.</returns>
        public IEnumerable<Review> GetReviewsByRating(int ratingId);

        /// <summary>
        /// Adds a new review after validating it.
        /// </summary>
        /// <param name="review">The review to add.</param>
        /// <returns>The added <see cref="Review"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the review is invalid.</exception>
        public Review AddReview(Review review);

        /// <summary>
        /// Deletes a review by its unique identifier.
        /// </summary>
        /// <param name="reviewId">The review identifier.</param>
        public void DeleteReviewById(int reviewId);
    }
}
