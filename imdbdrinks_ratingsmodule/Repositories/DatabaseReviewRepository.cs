namespace imdbdrinks_ratingsmodule.Repositories
{
    using System;
    using System.Collections.Generic;
    using imdbdrinks_ratingsmodule.Domain;
    using imdbdrinks_ratingsmodule.Repositories.Queries;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;

    public class DatabaseReviewRepository : IReviewRepository
    {
        private readonly DatabaseConnection databaseConnection;

        public DatabaseReviewRepository(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
        }

        public void DeleteReviewById(int reviewId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var command = new SqlCommand(ReviewQueries.DeleteReviewByIdQuery, connection);
            command.Parameters.AddWithValue("@ReviewId", reviewId);
            command.ExecuteNonQuery();
        }

        public IEnumerable<Review> GetAllReviews()
        {
            var reviews = new List<Review>();

            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var command = new SqlCommand(ReviewQueries.GetAllReviewsQuery, connection);

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
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var command = new SqlCommand(ReviewQueries.GetReviewByIdQuery, connection);
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

            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var command = new SqlCommand(ReviewQueries.GetReviewsByRatingIdQuery, connection);
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
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using (var checkCommand = new SqlCommand(ReviewQueries.ExistsReviewByIdQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@ReviewId", review.ReviewId);
                var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;

                if (!exists)
                {
                    using var insertCommand = new SqlCommand(
                        ReviewQueries.AddReviewQuery,
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
                        ReviewQueries.UpdateReviewQuery,
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
