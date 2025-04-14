// <copyright file="DatabaseReviewRepositoryHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule.Repositories
{
    using System;
    using System.Collections.Generic;
    using imdbdrinks_ratingsmodule.Constants.ErrorMessages;
    using imdbdrinks_ratingsmodule.Domain;
    using imdbdrinks_ratingsmodule.Queries;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Helper class for creating SQL commands related to Reviews.
    /// </summary>
    public static class DatabaseReviewRepositoryHelper
    {
        /// <summary>
        /// Creates an SQL command to add a review.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <param name="review">The review object to be added.</param>
        /// <returns>An SQL command to add a review.</returns>
        public static SqlCommand CreateAddReviewCommand(SqlConnection connection, Review review)
        {
            SqlCommand addCommand = new SqlCommand(ReviewQueries.AddReviewQuery, connection);
            addCommand.Parameters.AddWithValue("@RatingId", review.RatingId);
            addCommand.Parameters.AddWithValue("@UserId", review.UserId);
            addCommand.Parameters.AddWithValue("@Content", review.Content);
            addCommand.Parameters.AddWithValue("@CreationDate", review.CreationDate);
            addCommand.Parameters.AddWithValue("@IsActive", review.IsActive);

            return addCommand;
        }

        /// <summary>
        /// Creates an SQL command to update a review.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <param name="review">The review object to be updated.</param>
        /// <returns>An SQL command to update a review.</returns>
        public static SqlCommand CreateUpdateReviewCommand(SqlConnection connection, Review review)
        {
            SqlCommand updateCommand = new SqlCommand(ReviewQueries.UpdateReviewQuery, connection);
            updateCommand.Parameters.AddWithValue("@ReviewId", review.ReviewId);
            updateCommand.Parameters.AddWithValue("@RatingId", review.RatingId);
            updateCommand.Parameters.AddWithValue("@UserId", review.UserId);
            updateCommand.Parameters.AddWithValue("@Content", review.Content);
            updateCommand.Parameters.AddWithValue("@CreationDate", review.CreationDate);
            updateCommand.Parameters.AddWithValue("@IsActive", review.IsActive);

            return updateCommand;
        }

        /// <summary>
        /// Creates an SQL command to check if a review with the given ID exists.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <param name="reviewId">The review ID to check.</param>
        /// <returns>An SQL command to check if a review exists.</returns>
        public static SqlCommand CreateCheckIfReviewWithIdExistsCommand(SqlConnection connection, int reviewId)
        {
            SqlCommand existsCommand = new SqlCommand(ReviewQueries.CheckIfIdExistsQuery, connection);
            existsCommand.Parameters.AddWithValue("@ReviewId", reviewId);
            return existsCommand;
        }

        /// <summary>
        /// Creates an SQL command to get reviews by rating ID.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <param name="ratingId">The rating ID to fetch reviews for.</param>
        /// <returns>An SQL command to get reviews by rating ID.</returns>
        public static SqlCommand CreateGetReviewsByRatingIdCommand(SqlConnection connection, int ratingId)
        {
            SqlCommand getReviewsCommand = new SqlCommand(ReviewQueries.GetReviewsByRatingIdQuery, connection);
            getReviewsCommand.Parameters.AddWithValue("@RatingId", ratingId);

            return getReviewsCommand;
        }

        /// <summary>
        /// Creates an SQL command to get a review by its ID.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <param name="reviewId">The review ID to retrieve.</param>
        /// <returns>An SQL command to get a review by ID.</returns>
        public static SqlCommand CreateGetReviewByIdCommand(SqlConnection connection, int reviewId)
        {
            SqlCommand getReviewCommand = new SqlCommand(ReviewQueries.GetReviewByIdQuery, connection);
            getReviewCommand.Parameters.AddWithValue("@ReviewId", reviewId);

            return getReviewCommand;
        }

        /// <summary>
        /// Creates an SQL command to get all reviews.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <returns>An SQL command to get all reviews.</returns>
        public static SqlCommand CreateGetAllReviewsCommand(SqlConnection connection)
        {
            SqlCommand getAllReviewsCommand = new SqlCommand(ReviewQueries.GetAllReviewsQuery, connection);

            return getAllReviewsCommand;
        }

        /// <summary>
        /// Creates an SQL command to delete a review by its ID.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <param name="reviewId">The review ID to delete.</param>
        /// <returns>An SQL command to delete a review by its ID.</returns>
        public static SqlCommand CreateDeleteReviewById(SqlConnection connection, int reviewId)
        {
            SqlCommand deleteCommand = new SqlCommand(ReviewQueries.DeleteReviewByIdQuery, connection);
            deleteCommand.Parameters.AddWithValue("@ReviewId", reviewId);

            return deleteCommand;
        }

        /// <summary>
        /// Reads multiple reviews from the SQL data reader.
        /// </summary>
        /// <param name="reader">The SQL data reader.</param>
        /// <returns>A list of reviews.</returns>
        public static IEnumerable<Review> ExhaustReviewReader(SqlDataReader reader)
        {
            var reviews = new List<Review>();
            while (reader.Read())
            {
                reviews.Add(new Review
                {
                    ReviewId = reader.GetInt32(reader.GetOrdinal("ReviewId")),
                    RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Content = reader.GetString(reader.GetOrdinal("Content")),
                    CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                });
            }

            return reviews;
        }

        /// <summary>
        /// Reads a single review from the SQL data reader.
        /// </summary>
        /// <param name="reader">The SQL data reader.</param>
        /// <returns>A single review.</returns>
        /// <exception cref="InvalidOperationException">Thrown when more than one review is found.</exception>
        public static Review ExhaustSingleReviewReader(SqlDataReader reader)
        {
            if (reader.Read())
            {
                var review = new Review
                {
                    ReviewId = reader.GetInt32(reader.GetOrdinal("ReviewId")),
                    RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Content = reader.GetString(reader.GetOrdinal("Content")),
                    CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                };

                if (reader.Read()) // Check if there's a second one
                {
                    throw new InvalidOperationException(ReviewRepositoryErrorMessages.ExhaustSingleReviewReaderMultipleReviewsFound);
                }

                return review;
            }

            throw new InvalidOperationException(ReviewRepositoryErrorMessages.ExhaustSingleReviewReaderInvalidReader);
        }
    }
}
