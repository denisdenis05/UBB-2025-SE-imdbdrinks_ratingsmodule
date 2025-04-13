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

        public void Delete(int ratingId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var deleteRatingByIdCommand = DatabaseRatingRepositoryHelper.CreateDeleteRatingById(connection, ratingId);
            deleteRatingByIdCommand.ExecuteNonQuery();
        }

        public IEnumerable<Rating> FindAll()
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getAllRatingsCommand = DatabaseRatingRepositoryHelper.CreateGetAllRatingsCommand(connection);
            using var reader = getAllRatingsCommand.ExecuteReader();

            return DatabaseRatingRepositoryHelper.ExhaustRatingReader(reader);
        }

        public Rating FindById(int ratingId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getRatingByIdCommand = DatabaseRatingRepositoryHelper.CreateGetRatingByIdCommand(connection, ratingId);
            using var reader = getRatingByIdCommand.ExecuteReader();

            return DatabaseRatingRepositoryHelper.ExhaustSingleRatingReader(reader);
        }

        public IEnumerable<Rating> FindByProductId(int productId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getRatingsByProductIdCommand = DatabaseRatingRepositoryHelper.CreateGetRatingsByProductIdCommand(connection, productId);
            using var reader = getRatingsByProductIdCommand.ExecuteReader();

            return DatabaseRatingRepositoryHelper.ExhaustRatingReader(reader);
        }

        public Rating Add(Rating rating)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var addCommand = DatabaseRatingRepositoryHelper.CreateAddRatingCommand(connection, rating);
            rating.RatingId = (int)addCommand.ExecuteScalar();

            return rating;
        }

        public Rating Update(Rating rating)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var updateCommand = DatabaseRatingRepositoryHelper.CreateUpdateRatingCommand(connection, rating);
            updateCommand.ExecuteNonQuery();

            return rating;
        }

        public Rating Save(Rating rating)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var checkIfExistsCommand = DatabaseRatingRepositoryHelper.CreateCheckIfRatingWithIdExistsCommand(connection, rating.RatingId);
            var exists = Convert.ToBoolean(checkIfExistsCommand.ExecuteScalar());

            if (exists)
            {
                return Update(rating);
            }
            else
            {
                return Add(rating);
            }
        }
    }
}
