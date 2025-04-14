namespace imdbdrinks_ratingsmodule.Repositories
{
    using System;
    using System.Collections.Generic;
    using imdbdrinks_ratingsmodule.Constants.ErrorMessages;
    using imdbdrinks_ratingsmodule.Domain;
    using imdbdrinks_ratingsmodule.Queries;
    using Microsoft.Data.SqlClient;

    public static class DatabaseReviewRepositoryHelper
    {
        public static SqlCommand CreateAddReviewCommand(SqlConnection connection, Review review)
        {
            SqlCommand addCommand = new (ReviewQueries.AddReviewQuery, connection);
            addCommand.Parameters.AddWithValue("@RatingId", review.RatingId);
            addCommand.Parameters.AddWithValue("@UserId", review.UserId);
            addCommand.Parameters.AddWithValue("@Content", review.Content);
            addCommand.Parameters.AddWithValue("@CreationDate", review.CreationDate);
            addCommand.Parameters.AddWithValue("@IsActive", review.IsActive);

            return addCommand;
        }

        public static SqlCommand CreateUpdateReviewCommand(SqlConnection connection, Review review)
        {
            SqlCommand updateCommand = new (ReviewQueries.UpdateReviewQuery, connection);
            updateCommand.Parameters.AddWithValue("@ReviewId", review.ReviewId);
            updateCommand.Parameters.AddWithValue("@RatingId", review.RatingId);
            updateCommand.Parameters.AddWithValue("@UserId", review.UserId);
            updateCommand.Parameters.AddWithValue("@Content", review.Content);
            updateCommand.Parameters.AddWithValue("@CreationDate", review.CreationDate);
            updateCommand.Parameters.AddWithValue("@IsActive", review.IsActive);

            return updateCommand;
        }

        public static SqlCommand CreateCheckIfReviewWithIdExistsCommand(SqlConnection connection, int reviewId)
        {
            SqlCommand existsCommand = new(ReviewQueries.CheckIfIdExistsQuery, connection);
            existsCommand.Parameters.AddWithValue("@ReviewId", reviewId);
            return existsCommand;
        }

        public static SqlCommand CreateGetReviewsByRatingIdCommand(SqlConnection connection, int ratingId)
        {
            SqlCommand getReviewsCommand = new (ReviewQueries.GetReviewsByRatingIdQuery, connection);
            getReviewsCommand.Parameters.AddWithValue("@RatingId", ratingId);

            return getReviewsCommand;
        }

        public static SqlCommand CreateGetReviewByIdCommand(SqlConnection connection, int reviewId)
        {
            SqlCommand getReviewCommand = new (ReviewQueries.GetReviewByIdQuery, connection);
            getReviewCommand.Parameters.AddWithValue("@ReviewId", reviewId);

            return getReviewCommand;
        }

        public static SqlCommand CreateGetAllReviewsCommand(SqlConnection connection)
        {
            SqlCommand getAllReviewsCommand = new (ReviewQueries.GetAllReviewsQuery, connection);

            return getAllReviewsCommand;
        }

        public static SqlCommand CreateDeleteReviewById(SqlConnection connection, int reviewId)
        {
            SqlCommand deleteCommand = new (ReviewQueries.DeleteReviewByIdQuery, connection);
            deleteCommand.Parameters.AddWithValue("@ReviewId", reviewId);

            return deleteCommand;
        }

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
    }
}
