// <copyright file="ReviewService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule.Services
{
    using System;
    using System.Collections.Generic;
    using imdbdrinks_ratingsmodule.Constants.ErrorMessages;
    using imdbdrinks_ratingsmodule.Domain;
    using imdbdrinks_ratingsmodule.Repositories;

    /// <summary>
    /// Provides service-level operations for managing reviews.
    /// </summary>
    public class ReviewService
    {
        private readonly IReviewRepository reviewRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewService"/> class.
        /// </summary>
        /// <param name="reviewRepository">The review repository dependency.</param>
        public ReviewService(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }

        /// <summary>
        /// Retrieves all reviews associated with a specific rating.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>A collection of <see cref="Review"/> instances for the rating.</returns>
        public virtual IEnumerable<Review> GetReviewsByRating(int ratingId)
        {
            return this.reviewRepository.GetReviewsByRatingId(ratingId);
        }

        /// <summary>
        /// Adds a new review after validating it.
        /// </summary>
        /// <param name="review">The review to add.</param>
        /// <returns>The added <see cref="Review"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown when the review is invalid.</exception>
        public virtual Review AddReview(Review review)
        {
            if (!review.IsValid())
            {
                throw new ArgumentException(ReviewServiceErrorMessages.InvalidReview);
            }

            review.Activate();
            return this.reviewRepository.AddOrUpdateReview(review);
        }

        /// <summary>
        /// Deletes a review by its unique identifier.
        /// </summary>
        /// <param name="reviewId">The review identifier.</param>
        public virtual void DeleteReviewById(int reviewId)
        {
            this.reviewRepository.DeleteReviewById(reviewId);
        }
    }
}
