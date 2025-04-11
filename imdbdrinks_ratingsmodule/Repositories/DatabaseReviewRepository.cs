using imdbdrinks_ratingsmodule.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace imdbdrinks_ratingsmodule.Repositories
{
    class DatabaseReviewRepository : IReviewRepository
    {
        private readonly DatabaseConnection databaseConnection;

        public DatabaseReviewRepository(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
        }

        public void Delete(long reviewId)
        {
            try
            {
                using var connection = this.databaseConnection.CreateConnection();
                using var command = connection.CreateCommand();

                command.CommandText = "DELETE FROM Reviews WHERE ReviewId = @ReviewId";
                command.Parameters.AddWithValue("@ReviewId", reviewId);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Failed to delete review", ex);
            }
        }

        public IEnumerable<Review> FindAll()
        {
            var reviews = new List<Review>();

            try
            {
                using var connection = this.databaseConnection.CreateConnection();
                using var command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM Reviews";

                connection.Open();
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reviews.Add(new Review {
                        ReviewId = reader.GetInt32(reader.GetOrdinal("ReviewId")),
                        RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        Content = reader.GetString(reader.GetOrdinal("Content")),
                        CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                        IsActive = reader.GetByte(reader.GetOrdinal("IsActive")) == 1,
                    });
                }

                return reviews;
            }
            catch (SqlException ex)
            {
                throw new Exception("Failed to retrieve reviews", ex);
            }
        }

        public Review FindById(long reviewId)
        {
            try
            {
                using var connection = this.databaseConnection.CreateConnection();
                using var command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM Reviews WHERE ReviewId = @ReviewId";
                command.Parameters.AddWithValue("@ReviewId", reviewId);

                connection.Open();
                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Review
                    {
                        ReviewId = reader.GetInt32(reader.GetOrdinal("ReviewId")),
                        RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        Content = reader.GetString(reader.GetOrdinal("Content")),
                        CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                        IsActive = reader.GetByte(reader.GetOrdinal("IsActive")) == 1,
                    };
                }

                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Failed to retrieve review", ex);
            }
        }

        public IEnumerable<Review> FindByRatingId(long ratingId)
        {
            var reviews = new List<Review>();

            try {
                using var connection = this.databaseConnection.CreateConnection();
                using var command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM Reviews WHERE RatingId = @RatingId";
                command.Parameters.AddWithValue("@RatingId", ratingId);

                connection.Open();
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reviews.Add(new Review
                    {
                        ReviewId = reader.GetInt32(reader.GetOrdinal("ReviewId")),
                        RatingId = reader.GetInt32(reader.GetOrdinal("RatingId")),
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        Content = reader.GetString(reader.GetOrdinal("Content")),
                        CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate")),
                        IsActive = reader.GetByte(reader.GetOrdinal("IsActive")) == 1,
                    });
                }

                return reviews;
            }
            catch (SqlException ex)
            {
                throw new Exception("Failed to retrieve reviews", ex);
            }
        }

        public Review Save(Review review)
        {
            try
            {
                using var connection = this.databaseConnection.CreateConnection();
                using var command = connection.CreateCommand();

                // check if review exists
                var exists = this.FindById(review.ReviewId) != null;

                if (exists)
                {
                    // update review
                    command.CommandText = @"
                        UPDATE Reviews
                        SET RatingId = @RatingId,
                            UserId = @UserId,
                            Content = @Content,
                            CreationDate = @CreationDate,
                            IsActive = @IsActive
                        WHERE ReviewId = @ReviewId";
                    command.Parameters.AddWithValue("@ReviewId", review.ReviewId);
                    command.Parameters.AddWithValue("@RatingId", review.RatingId);
                    command.Parameters.AddWithValue("@UserId", review.UserId);
                    command.Parameters.AddWithValue("@Content", review.Content);
                    command.Parameters.AddWithValue("@CreationDate", review.CreationDate);
                    command.Parameters.AddWithValue("@IsActive", review.IsActive);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                else
                {
                    // create review
                    command.CommandText = @"
                        INSERT INTO Reviews (RatingId, UserId, Content, CreationDate, IsActive)
                        OUTPUT INSERTED.ReviewId
                        VALUES (@RatingId, @UserId, @Content, @CreationDate, @IsActive)";
                    command.Parameters.AddWithValue("@RatingId", review.RatingId);
                    command.Parameters.AddWithValue("@UserId", review.UserId);
                    command.Parameters.AddWithValue("@Content", review.Content);
                    command.Parameters.AddWithValue("@CreationDate", review.CreationDate);
                    command.Parameters.AddWithValue("@IsActive", review.IsActive);

                    connection.Open();
                    review.ReviewId = (int)command.ExecuteScalar();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Failed to save review", ex);
            }

            return review;
        }
    }
}
