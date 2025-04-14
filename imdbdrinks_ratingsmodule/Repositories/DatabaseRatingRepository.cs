using imdbdrinks_ratingsmodule.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace imdbdrinks_ratingsmodule.Repositories
{
    public class DatabaseRatingRepository : IRatingRepository
    {
        private readonly DatabaseConnection databaseConnection;

        public DatabaseRatingRepository(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public void DeleteRatingById(int ratingId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var deleteRatingByIdCommand = DatabaseRatingRepositoryHelper.CreateDeleteRatingById(connection, ratingId);
            deleteRatingByIdCommand.ExecuteNonQuery();
        }

        public IEnumerable<Rating> GetAllRatings()
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getAllRatingsCommand = DatabaseRatingRepositoryHelper.CreateGetAllRatingsCommand(connection);
            using var reader = getAllRatingsCommand.ExecuteReader();

            return DatabaseRatingRepositoryHelper.ExhaustRatingReader(reader);
        }

        public Rating GetRatingById(int ratingId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getRatingByIdCommand = DatabaseRatingRepositoryHelper.CreateGetRatingByIdCommand(connection, ratingId);
            using var reader = getRatingByIdCommand.ExecuteReader();

            return DatabaseRatingRepositoryHelper.ExhaustSingleRatingReader(reader);
        }

        public IEnumerable<Rating> GetRatingsByProductId(int productId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getRatingsByProductIdCommand = DatabaseRatingRepositoryHelper.CreateGetRatingsByProductIdCommand(connection, productId);
            using var reader = getRatingsByProductIdCommand.ExecuteReader();

            return DatabaseRatingRepositoryHelper.ExhaustRatingReader(reader);
        }

        public Rating AddRating(Rating rating)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var addCommand = DatabaseRatingRepositoryHelper.CreateAddRatingCommand(connection, rating);
            rating.RatingId = (int)addCommand.ExecuteScalar();

            return rating;
        }

        public Rating UpdateRating(Rating rating)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var updateCommand = DatabaseRatingRepositoryHelper.CreateUpdateRatingCommand(connection, rating);
            updateCommand.ExecuteNonQuery();

            return rating;
        }

        public Rating AddOrUpdateRating(Rating rating)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var checkIfExistsCommand = DatabaseRatingRepositoryHelper.CreateCheckIfRatingWithIdExistsCommand(connection, rating.RatingId);
            var existsRating = Convert.ToBoolean(checkIfExistsCommand.ExecuteScalar());

            if (existsRating)
            {
                return UpdateRating(rating);
            }
            else
            {
                return AddRating(rating);
            }
        }
    }
}
