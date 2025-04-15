// <copyright file="RatingService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using imdbdrinks_ratingsmodule.Constants.ErrorMessages;
    using imdbdrinks_ratingsmodule.Domain;
    using imdbdrinks_ratingsmodule.Repositories;

    /// <summary>
    /// Provides service-level operations for managing ratings.
    /// </summary>
    public class RatingService
    {
        private readonly IRatingRepository ratingRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingService"/> class.
        /// </summary>
        /// <param name="ratingRepository">The rating repository dependency.</param>
        public RatingService(IRatingRepository ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }

        /// <summary>
        /// Retrieves a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>The corresponding rating or null if it doesnt exist.<see cref="Rating"/>.</returns>
        public virtual Rating? GetRatingById(int ratingId) => this.ratingRepository.GetRatingById(ratingId);

        /// <summary>
        /// Retrieves all ratings associated with a specific product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>A collection of <see cref="Rating"/> instances for the product.</returns>
        public virtual IEnumerable<Rating> GetRatingsByProduct(int productId) => this.ratingRepository.GetRatingsByProductId(productId);

        /// <summary>
        /// Creates a new rating.
        /// </summary>
        /// <param name="rating">The rating to create.</param>
        /// <returns>The created <see cref="Rating"/> instance.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the rating is invalid.</exception>
        public virtual Rating CreateRating(Rating rating)
        {
            if (!rating.IsValid())
            {
                throw new System.ArgumentException(RatingServiceErrorMessages.InvalidRatingValue);
            }

            rating.RatingDate = System.DateTime.Now;
            rating.IsActive = true;

            return this.ratingRepository.AddRating(rating);
        }

        /// <summary>
        /// Updates an existing rating.
        /// </summary>
        /// <param name="rating">The rating to update.</param>
        /// <returns>The updated <see cref="Rating"/> instance.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the rating is invalid.</exception>
        public virtual Rating UpdateRating(Rating rating)
        {
            if (!rating.IsValid())
            {
                throw new System.ArgumentException(RatingServiceErrorMessages.InvalidRatingValue);
            }

            return this.ratingRepository.UpdateRating(rating);
        }

        /// <summary>
        /// Deletes a rating by its unique identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        public virtual void DeleteRatingById(int ratingId) => this.ratingRepository.DeleteRatingById(ratingId);

        /// <summary>
        /// Calculates the average value of all active ratings for a product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>The average rating value, or 0 if no ratings are present.</returns>
        public virtual double GetAverageRating(int productId)
        {
            var ratings = this.ratingRepository.GetRatingsByProductId(productId).Where(r => r.IsActive);

            if (!ratings.Any())
            {
                return 0;
            }

            return ratings.Average(r => r.RatingValue);
        }
    }
}