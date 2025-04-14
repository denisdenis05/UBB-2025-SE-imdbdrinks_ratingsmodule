using imdbdrinks_ratingsmodule.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace imdbdrinks_ratingsmodule.Repositories
{
    /// <summary>
    /// Repository class for handling operations related to ratings in the database.
    /// </summary>
    public class DatabaseRatingRepository : IRatingRepository
    {
        private readonly DatabaseConnection _databaseConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseRatingRepository"/> class.
        /// </summary>
        /// <param name="databaseConnection">The database connection.</param>
        public DatabaseRatingRepository(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        /// <summary>
        /// Deletes a rating by its identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        public void DeleteRatingById(int ratingId)
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var deleteRatingByIdCommand = DatabaseRatingRepositoryHelper.CreateDeleteRatingById(connection, ratingId);
            deleteRatingByIdCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Retrieves all ratings.
        /// </summary>
        /// <returns>A collection of ratings.</returns>
        public IEnumerable<Rating> GetAllRatings()
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var getAllRatingsCommand = DatabaseRatingRepositoryHelper.CreateGetAllRatingsCommand(connection);
            using var reader = getAllRatingsCommand.ExecuteReader();

            return DatabaseRatingRepositoryHelper.ExhaustRatingReader(reader);
        }

        /// <summary>
        /// Retrieves a rating by its identifier.
        /// </summary>
        /// <param name="ratingId">The rating identifier.</param>
        /// <returns>The rating.</returns>
        public Rating GetRatingById(int ratingId)
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var getRatingByIdCommand = DatabaseRatingRepositoryHelper.CreateGetRatingByIdCommand(connection, ratingId);
            using var reader = getRatingByIdCommand.ExecuteReader();

            return DatabaseRatingRepositoryHelper.ExhaustSingleRatingReader(reader);
        }

        /// <summary>
        /// Retrieves ratings by product identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>A collection of ratings.</returns>
        public IEnumerable<Rating> GetRatingsByProductId(int productId)
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var getRatingsByProductIdCommand = DatabaseRatingRepositoryHelper.CreateGetRatingsByProductIdCommand(connection, productId);
            using var reader = getRatingsByProductIdCommand.ExecuteReader();

            return DatabaseRatingRepositoryHelper.ExhaustRatingReader(reader);
        }

        /// <summary>
        /// Adds a new rating to the database.
        /// </summary>
        /// <param name="rating">The rating to add.</param>
        /// <returns>The added rating with its generated identifier.</returns>
        public Rating AddRating(Rating rating)
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var addCommand = DatabaseRatingRepositoryHelper.CreateAddRatingCommand(connection, rating);
            rating.RatingId = (int)addCommand.ExecuteScalar();

            return rating;
        }

        /// <summary>
        /// Updates an existing rating in the database.
        /// </summary>
        /// <param name="rating">The rating to update.</param>
        /// <returns>The updated rating.</returns>
        public Rating UpdateRating(Rating rating)
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var updateCommand = DatabaseRatingRepositoryHelper.CreateUpdateRatingCommand(connection, rating);
            updateCommand.ExecuteNonQuery();

            return rating;
        }

        /// <summary>
        /// Adds or updates a rating in the database.
        /// </summary>
        /// <param name="rating">The rating to add or update.</param>
        /// <returns>The added or updated rating.</returns>
        public Rating AddOrUpdateRating(Rating rating)
        {
            using var connection = _databaseConnection.CreateConnection();
            connection.Open();

            using var checkIfExistsCommand = DatabaseRatingRepositoryHelper.CreateCheckIfRatingWithIdExistsCommand(connection, rating.RatingId);
            var existsRating = Convert.ToBoolean(checkIfExistsCommand.ExecuteScalar());

            if (existsRating)
            {
                return UpdateRating(rating);
            }

            return AddRating(rating);
        }
    }
}
