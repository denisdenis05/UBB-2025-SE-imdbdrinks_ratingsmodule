namespace imdbdrinks_ratingsmodule.Repositories
{
    using System;
    using System.Collections.Generic;
    using imdbdrinks_ratingsmodule.Constants.ErrorMessages;
    using imdbdrinks_ratingsmodule.Domain;
    using imdbdrinks_ratingsmodule.Queries;
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Helper class for creating SQL commands related to Ratings.
    /// </summary>
    public static class DatabaseRatingRepositoryHelper
    {
        /// <summary>
        /// Creates an SQL command to add a rating.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <param name="rating">The rating object to be added.</param>
        /// <returns>An SQL command to add a rating.</returns>
        public static SqlCommand CreateAddRatingCommand(SqlConnection connection, Rating rating)
        {
            SqlCommand addCommand = new SqlCommand(RatingQueries.AddRatingQuery, connection);
            addCommand.Parameters.AddWithValue("@ProductId", rating.ProductId);
            addCommand.Parameters.AddWithValue("@UserId", rating.UserId);
            addCommand.Parameters.AddWithValue("@RatingValue", rating.RatingValue);
            addCommand.Parameters.AddWithValue("@RatingDate", rating.RatingDate);
            addCommand.Parameters.AddWithValue("@IsActive", rating.IsActive);

            return addCommand;
        }

        /// <summary>
        /// Creates an SQL command to update a rating.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <param name="rating">The rating object to be updated.</param>
        /// <returns>An SQL command to update a rating.</returns>
        public static SqlCommand CreateUpdateRatingCommand(SqlConnection connection, Rating rating)
        {
            SqlCommand updateCommand = new SqlCommand(RatingQueries.UpdateRatingQuery, connection);
            updateCommand.Parameters.AddWithValue("@RatingId", rating.RatingId);
            updateCommand.Parameters.AddWithValue("@ProductId", rating.ProductId);
            updateCommand.Parameters.AddWithValue("@UserId", rating.UserId);
            updateCommand.Parameters.AddWithValue("@RatingValue", rating.RatingValue);
            updateCommand.Parameters.AddWithValue("@RatingDate", rating.RatingDate);
            updateCommand.Parameters.AddWithValue("@IsActive", rating.IsActive);

            return updateCommand;
        }

        /// <summary>
        /// Creates an SQL command to check if a rating with the given ID exists.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <param name="ratingId">The rating ID to check.</param>
        /// <returns>An SQL command to check if a rating exists.</returns>
        public static SqlCommand CreateCheckIfRatingWithIdExistsCommand(SqlConnection connection, int ratingId)
        {
            SqlCommand existsCommand = new SqlCommand(RatingQueries.CheckIfRatingWithIdExistsQuery, connection);
            existsCommand.Parameters.AddWithValue("@RatingId", ratingId);
            return existsCommand;
        }

        /// <summary>
        /// Creates an SQL command to get ratings by product ID.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <param name="productId">The product ID.</param>
        /// <returns>An SQL command to get ratings by product ID.</returns>
        public static SqlCommand CreateGetRatingsByProductIdCommand(SqlConnection connection, int productId)
        {
            SqlCommand getRatingsCommand = new SqlCommand(RatingQueries.GetRatingsByProductIdQuery, connection);
            getRatingsCommand.Parameters.AddWithValue("@ProductId", productId);

            return getRatingsCommand;
        }

        /// <summary>
        /// Creates an SQL command to get a rating by its ID.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <param name="ratingId">The rating ID.</param>
        /// <returns>An SQL command to get a rating by its ID.</returns>
        public static SqlCommand CreateGetRatingByIdCommand(SqlConnection connection, int ratingId)
        {
            SqlCommand getRatingCommand = new SqlCommand(RatingQueries.GetRatingByIdQuery, connection);
            getRatingCommand.Parameters.AddWithValue("@RatingId", ratingId);

            return getRatingCommand;
        }

        /// <summary>
        /// Creates an SQL command to get all ratings.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <returns>An SQL command to get all ratings.</returns>
        public static SqlCommand CreateGetAllRatingsCommand(SqlConnection connection)
        {
            return new SqlCommand(RatingQueries.GetAllRatingsQuery, connection);
        }

        /// <summary>
        /// Creates an SQL command to delete a rating by its ID.
        /// </summary>
        /// <param name="connection">The SQL connection.</param>
        /// <param name="ratingId">The rating ID to be deleted.</param>
        /// <returns>An SQL command to delete a rating by its ID.</returns>
        public static SqlCommand CreateDeleteRatingById(SqlConnection connection, int ratingId)
        {
            SqlCommand deleteCommand = new SqlCommand(RatingQueries.DeleteRatingQuery, connection);
            deleteCommand.Parameters.AddWithValue("@RatingId", ratingId);

            return deleteCommand;
        }

        /// <summary>
        /// Reads ratings from the SQL data reader.
        /// </summary>
        /// <param name="reader">The SQL data reader.</param>
        /// <returns>A collection of ratings.</returns>
        public static IEnumerable<Rating> ExhaustRatingReader(SqlDataReader reader)
        {
            var ratings = new List<Rating>();
            while (reader.Read())
            {
                ratings.Add(new Rating
                {
                    RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    RatingValue = reader.GetDouble(reader.GetOrdinal("RatingValue")),
                    RatingDate = reader.GetDateTime(reader.GetOrdinal("RatingDate")),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                });
            }

            return ratings;
        }

        /// <summary>
        /// Reads a single rating from the SQL data reader.
        /// </summary>
        /// <param name="reader">The SQL data reader.</param>
        /// <returns>A single rating object.</returns>
        public static Rating ExhaustSingleRatingReader(SqlDataReader reader)
        {
            if (reader.Read())
            {
                var rating = new Rating
                {
                    RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    RatingValue = reader.GetDouble(reader.GetOrdinal("RatingValue")),
                    RatingDate = reader.GetDateTime(reader.GetOrdinal("RatingDate")),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                };

                if (reader.Read()) // Check if there's a second one
                {
                    throw new InvalidOperationException(RatingRepositoryErrorMessages.ExhaustSingleRatingReaderMultipleRatingsFound);
                }

                return rating;
            }

            throw new InvalidOperationException(RatingRepositoryErrorMessages.ExhaustSingleRatingReaderInvalidReader);
        }
    }
}
