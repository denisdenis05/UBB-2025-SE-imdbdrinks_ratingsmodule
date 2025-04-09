namespace imdbdrinks_ratingsmodule.Repositories
{
    using System;
    using System.Collections.Generic;
    using imdbdrinks_ratingsmodule.Domain;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;

    public class DatabaseReviewRepository : IReviewRepository
    {
        private readonly string connectionString;

        public DatabaseReviewRepository(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            this.connectionString = configuration["DbConnection"]
                ?? throw new InvalidOperationException("DbConnection configuration is missing or null.");
        }

        public void DeleteReviewById(int reviewId)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = new SqlCommand("DELETE FROM Reviews WHERE ReviewId = @ReviewId", connection);
            command.Parameters.AddWithValue("@ReviewId", reviewId);
            command.ExecuteNonQuery();
        }

        public IEnumerable<Review> GetAllReviews()
        {
            var reviews = new List<Review>();

            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = new SqlCommand("SELECT * FROM Reviews", connection);

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
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                });
            }

            return reviews;
        }

        public Review GetReviewById(int reviewId)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = new SqlCommand("SELECT * FROM Reviews WHERE ReviewId = @ReviewId", connection);
            command.Parameters.AddWithValue("@ReviewId", reviewId);

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
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                };
            }

            throw new Exception($"Review with ID {reviewId} not found.");
        }

        public IEnumerable<Review> GetReviewsByRatingId(int ratingId)
        {
            var reviews = new List<Review>();

            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using var command = new SqlCommand("SELECT * FROM Reviews WHERE RatingId = @RatingId", connection);
            command.Parameters.AddWithValue("@RatingId", ratingId);

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
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                });
            }

            return reviews;
        }

        public Review AddOrUpdateReview(Review review)
        {
            using var connection = new SqlConnection(this.connectionString);
            connection.Open();

            using (var checkCommand = new SqlCommand("SELECT COUNT(*) FROM Reviews WHERE ReviewId = @ReviewId", connection))
            {
                checkCommand.Parameters.AddWithValue("@ReviewId", review.ReviewId);
                var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;

                if (!exists)
                {
                    using var insertCommand = new SqlCommand(
                        @"INSERT INTO Reviews (RatingId, UserId, Content, CreationDate, IsActive)
                        OUTPUT INSERTED.ReviewId
                        VALUES (@RatingId, @UserId, @Content, @CreationDate, @IsActive)",
                        connection);

                    insertCommand.Parameters.AddWithValue("@RatingId", review.RatingId);
                    insertCommand.Parameters.AddWithValue("@UserId", review.UserId);
                    insertCommand.Parameters.AddWithValue("@Content", review.Content);
                    insertCommand.Parameters.AddWithValue("@CreationDate", review.CreationDate);
                    insertCommand.Parameters.AddWithValue("@IsActive", review.IsActive);

                    review.ReviewId = (int)insertCommand.ExecuteScalar();
                }
                else
                {
                    using var updateCommand = new SqlCommand(
                        @"UPDATE Reviews
                        SET RatingId = @RatingId,
                            UserId = @UserId,
                            Content = @Content,
                            CreationDate = @CreationDate,
                            IsActive = @IsActive
                        WHERE ReviewId = @ReviewId",
                        connection);

                    updateCommand.Parameters.AddWithValue("@ReviewId", review.ReviewId);
                    updateCommand.Parameters.AddWithValue("@RatingId", review.RatingId);
                    updateCommand.Parameters.AddWithValue("@UserId", review.UserId);
                    updateCommand.Parameters.AddWithValue("@Content", review.Content);
                    updateCommand.Parameters.AddWithValue("@CreationDate", review.CreationDate);
                    updateCommand.Parameters.AddWithValue("@IsActive", review.IsActive);

                    updateCommand.ExecuteNonQuery();
                }
            }

            return review;
        }
    }
}
