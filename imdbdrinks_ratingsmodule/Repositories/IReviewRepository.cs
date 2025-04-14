// <copyright file="IReviewRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule.Repositories
{
    using System.Collections.Generic;
    using imdbdrinks_ratingsmodule.Domain;

    /// <summary>
    /// Provides methods for managing review entities in the data store.
    /// </summary>
    public interface IReviewRepository
    {
        /// <summary>
        /// Retrieves a review by its unique identifier.
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review.</param>
        /// <returns>The <see cref="Review"/> with the specified identifier.</returns>
        Review GetReviewById(int reviewId);

        /// <summary>
        /// Retrieves all reviews from the data store.
        /// </summary>
        /// <returns>An enumerable collection of all <see cref="Review"/> instances.</returns>
        IEnumerable<Review> GetAllReviews();

        /// <summary>
        /// Retrieves all reviews associated with a specific rating identifier.
        /// </summary>
        /// <param name="ratingId">The unique identifier of the rating.</param>
        /// <returns>An enumerable collection of <see cref="Review"/> instances related to the specified rating.</returns>
        IEnumerable<Review> GetReviewsByRatingId(int ratingId);

        /// <summary>
        /// Adds a new review to the data store.
        /// </summary>
        /// <param name="review">The <see cref="Review"/> to add.</param>
        /// <returns>The unique identifier of the added review.</returns>
        int AddReview(Review review);

        /// <summary>
        /// Updates an existing review in the data store.
        /// </summary>
        /// <param name="review">The <see cref="Review"/> with updated information.</param>
        /// <returns>The updated <see cref="Review"/>.</returns>
        Review UpdateReview(Review review);

        /// <summary>
        /// Adds a new review or updates an existing one in the data store.
        /// </summary>
        /// <param name="review">The <see cref="Review"/> to add or update.</param>
        /// <returns>The added or updated <see cref="Review"/>.</returns>
        Review AddOrUpdateReview(Review review);

        /// <summary>
        /// Deletes a review by its unique identifier.
        /// </summary>
        /// <param name="reviewId">The unique identifier of the review to delete.</param>
        void DeleteReviewById(int reviewId);
    }
}
