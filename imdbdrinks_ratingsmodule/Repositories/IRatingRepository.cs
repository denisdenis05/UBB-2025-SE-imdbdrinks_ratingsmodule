// <copyright file="IRatingRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule.Repositories
{
    using System.Collections.Generic;
    using imdbdrinks_ratingsmodule.Domain;

    /// <summary>
    /// Defines methods for managing rating entities in the data store.
    /// </summary>
    public interface IRatingRepository
    {
        /// <summary>
        /// Retrieves a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The unique identifier of the rating.</param>
        /// <returns>The <see cref="Rating"/> with the specified identifier.</returns>
        Rating? GetRatingById(int ratingId);

        /// <summary>
        /// Retrieves all ratings from the data store.
        /// </summary>
        /// <returns>An enumerable collection of all <see cref="Rating"/> instances.</returns>
        IEnumerable<Rating> GetAllRatings();

        /// <summary>
        /// Retrieves all ratings associated with a specific product identifier.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>An enumerable collection of <see cref="Rating"/> instances related to the specified product.</returns>
        IEnumerable<Rating> GetRatingsByProductId(int productId);

        /// <summary>
        /// Adds a new rating to the data store.
        /// </summary>
        /// <param name="rating">The <see cref="Rating"/> to add.</param>
        /// <returns>The added <see cref="Rating"/> with its assigned identifier.</returns>
        Rating AddRating(Rating rating);

        /// <summary>
        /// Updates an existing rating in the data store.
        /// </summary>
        /// <param name="rating">The <see cref="Rating"/> with updated information.</param>
        /// <returns>The updated <see cref="Rating"/>.</returns>
        Rating UpdateRating(Rating rating);

        /// <summary>
        /// Deletes a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The unique identifier of the rating to delete.</param>
        void DeleteRatingById(int ratingId);

        /// <summary>
        /// Adds a new rating or updates an existing one in the data store.
        /// </summary>
        /// <param name="rating">The <see cref="Rating"/> to add or update.</param>
        /// <returns>The added or updated <see cref="Rating"/>.</returns>
        Rating AddOrUpdateRating(Rating rating);
    }
}
