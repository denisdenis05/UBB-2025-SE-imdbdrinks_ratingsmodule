namespace imdbdrinks_ratingsmodule.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using imdbdrinks_ratingsmodule.Domain;
    using imdbdrinks_ratingsmodule.Queries;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    public class DatabaseReviewRepository : IReviewRepository
    {
        private readonly DatabaseConnection databaseConnection;

        public DatabaseReviewRepository(DatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public void DeleteReviewById(int reviewId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var command = DatabaseReviewRepositoryHelper.CreateDeleteReviewById(connection, reviewId);
            command.ExecuteNonQuery();
        }

        public IEnumerable<Review> GetAllReviews()
        {
            var reviews = new List<Review>();

            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var command = DatabaseReviewRepositoryHelper.CreateGetAllReviewsCommand(connection);

            using var reader = command.ExecuteReader();

            return DatabaseReviewRepositoryHelper.ExhaustReviewReader(reader);
        }

        public Review GetReviewById(int reviewId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var command = DatabaseReviewRepositoryHelper.CreateGetReviewByIdCommand(connection, reviewId);

            using var reader = command.ExecuteReader();

            return DatabaseReviewRepositoryHelper.ExhaustSingleReviewReader(reader);
        }

        public IEnumerable<Review> GetReviewsByRatingId(int ratingId)
        {
            var reviews = new List<Review>();

            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var command = DatabaseReviewRepositoryHelper.CreateGetReviewsByRatingIdCommand(connection, ratingId);

            using var reader = command.ExecuteReader();

            return DatabaseReviewRepositoryHelper.ExhaustReviewReader(reader);
        }

        public bool CheckIfReviewWithIdExists(int reviewId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var command = DatabaseReviewRepositoryHelper.CreateExistsReviewByIdCommand(connection, reviewId);

            // Execute the command and check if any rows are returned
            // if any rows are returned, it means the review exists
            return (int)command.ExecuteScalar() > 0;
        }

        public int AddReview(Review review)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var command = DatabaseReviewRepositoryHelper.CreateAddReviewCommand(connection, review);

            return (int)command.ExecuteScalar();
        }

        public Review UpdateReview(Review review)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var updateCommand = DatabaseReviewRepositoryHelper.CreateUpdateReviewCommand(connection, review);
            updateCommand.ExecuteNonQuery();

            return review;
        }

        public Review AddOrUpdateReview(Review review)
        {
            var reviewExists = this.CheckIfReviewWithIdExists(review.ReviewId);
            if (reviewExists)
            {
                this.UpdateReview(review);
            }
            else
            {
                review.ReviewId = this.AddReview(review);
            }

            return review;
        }
    }
}
