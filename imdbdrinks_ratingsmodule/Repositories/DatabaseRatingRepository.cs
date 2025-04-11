using imdbdrinks_ratingsmodule.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace imdbdrinks_ratingsmodule.Repositories
{
    class DatabaseRatingRepository : IRatingRepository
    {
        private readonly DatabaseConnection databaseConnection;

        public DatabaseRatingRepository(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
        }

        public void Delete(long ratingId)
        {
            try
            {
                using var connection = this.databaseConnection.CreateConnection();
                using var command = connection.CreateCommand();

                command.CommandText = "DELETE FROM Ratings WHERE RatingId = @RatingId";
                command.Parameters.AddWithValue("@RatingId", ratingId);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Failed to delete rating", ex);
            }
        }

        public IEnumerable<Rating> FindAll()
        {
            var ratings = new List<Rating>();

            try
            {
                using var connection = this.databaseConnection.CreateConnection();
                using var command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM Ratings";

                connection.Open();
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ratings.Add(new Rating
                    {
                        RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        RatingValue = reader.GetInt32(reader.GetOrdinal("RatingValue")),
                        RatingDate = reader.GetDateTime(reader.GetOrdinal("RatingDate")),
                        IsActive = reader.GetByte(reader.GetOrdinal("IsActive")) == 1,
                    });
                }

                return ratings;
            }
            catch (SqlException ex)
            {
                throw new Exception("Failed to retrieve ratings", ex);
            }
        }

        public Rating FindById(long ratingId)
        {
            try
            {
                using var connection = this.databaseConnection.CreateConnection();
                using var command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM Ratings WHERE RatingId = @RatingId";
                command.Parameters.AddWithValue("@RatingId", ratingId);

                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Rating
                    {
                        RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        RatingValue = reader.GetInt32(reader.GetOrdinal("RatingValue")),
                        RatingDate = reader.GetDateTime(reader.GetOrdinal("RatingDate")),
                        IsActive = reader.GetByte(reader.GetOrdinal("IsActive")) == 1,
                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Failed to retrieve rating", ex);
            }
        }

        public IEnumerable<Rating> FindByProductId(long productId)
        {
            var ratings = new List<Rating>();

            try
            {
                using var connection = this.databaseConnection.CreateConnection();
                using var command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM Ratings WHERE ProductId = @ProductId";
                command.Parameters.AddWithValue("@ProductId", productId);

                connection.Open();
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ratings.Add(new Rating
                    {
                        RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                        ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        RatingValue = reader.GetDouble(reader.GetOrdinal("RatingValue")),
                        RatingDate = reader.GetDateTime(reader.GetOrdinal("RatingDate")),
                        IsActive = reader.GetByte(reader.GetOrdinal("IsActive")) == 1,
                    });
                }

                return ratings;
            }
            catch (SqlException ex)
            {
                throw new Exception("Failed to retrieve ratings", ex);
            }
        }

        public Rating Save(Rating rating)
        {
            try
            {
                using var connection = this.databaseConnection.CreateConnection();
                using var command = connection.CreateCommand();

                // check if rating exists
                var exists = this.FindById(rating.RatingId) != null;

                if (exists)
                {
                    // update rating
                    command.CommandText = @"
                        UPDATE Ratings
                        SET ProductId = @ProductId, UserId = @UserId, RatingValue = @RatingValue, RatingDate = @RatingDate, IsActive = @IsActive
                        WHERE RatingId = @RatingId";
                    command.Parameters.AddWithValue("@RatingId", rating.RatingId);
                    command.Parameters.AddWithValue("@ProductId", rating.ProductId);
                    command.Parameters.AddWithValue("@UserId", rating.UserId);
                    command.Parameters.AddWithValue("@RatingValue", rating.RatingValue);
                    command.Parameters.AddWithValue("@RatingDate", rating.RatingDate);
                    command.Parameters.AddWithValue("@IsActive", rating.IsActive);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                else
                {
                    // create rating
                    command.CommandText = @"
                        INSERT INTO Ratings (ProductId, UserId, RatingValue, RatingDate, IsActive)
                        OUTPUT INSERTED.RatingId
                        VALUES (@ProductId, @UserId, @RatingValue, @RatingDate, @IsActive)";
                    command.Parameters.AddWithValue("@ProductId", rating.ProductId);
                    command.Parameters.AddWithValue("@UserId", rating.UserId);
                    command.Parameters.AddWithValue("@RatingValue", rating.RatingValue);
                    command.Parameters.AddWithValue("@RatingDate", rating.RatingDate);
                    command.Parameters.AddWithValue("@IsActive", rating.IsActive);

                    connection.Open();
                    rating.RatingId = (int)command.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Failed to save rating", ex);
            }

            return rating;
        }
    }
}
