namespace imdbdrinks_ratingsmodule.Repositories
{
    using System;
    using System.Collections.Generic;
    using imdbdrinks_ratingsmodule.Domain;

    public class DatabaseReviewRepository(DatabaseConnection databaseConnection) : IReviewRepository
    {
        private readonly DatabaseConnection databaseConnection = databaseConnection;

        public void DeleteReviewById(int reviewId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var deleteReviewByIdCommand = DatabaseReviewRepositoryHelper.CreateDeleteReviewById(connection, reviewId);
            deleteReviewByIdCommand.ExecuteNonQuery();
        }

        public IEnumerable<Review> GetAllReviews()
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getAllReviewsCommand = DatabaseReviewRepositoryHelper.CreateGetAllReviewsCommand(connection);

            using var reader = getAllReviewsCommand.ExecuteReader();

            return DatabaseReviewRepositoryHelper.ExhaustReviewReader(reader);
        }

        public Review GetReviewById(int reviewId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getReviewByIdCommand = DatabaseReviewRepositoryHelper.CreateGetReviewByIdCommand(connection, reviewId);

            using var reader = getReviewByIdCommand.ExecuteReader();

            return DatabaseReviewRepositoryHelper.ExhaustSingleReviewReader(reader);
        }

        public IEnumerable<Review> GetReviewsByRatingId(int ratingId)
        {
            var reviews = new List<Review>();

            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var getReviewsByRatingIdCommand = DatabaseReviewRepositoryHelper.CreateGetReviewsByRatingIdCommand(connection, ratingId);

            using var reader = getReviewsByRatingIdCommand.ExecuteReader();

            return DatabaseReviewRepositoryHelper.ExhaustReviewReader(reader);
        }

        public bool CheckIfReviewWithIdExists(int reviewId)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var checkIfReviewWithIdExistsCommand = DatabaseReviewRepositoryHelper.CreateCheckIfReviewWithIdExistsCommand(connection, reviewId);

            var doesReviewExist = Convert.ToBoolean(checkIfReviewWithIdExistsCommand.ExecuteScalar());

            return doesReviewExist;
        }

        public int AddReview(Review review)
        {
            using var connection = this.databaseConnection.CreateConnection();
            connection.Open();

            using var addCommand = DatabaseReviewRepositoryHelper.CreateAddReviewCommand(connection, review);

            return (int)addCommand.ExecuteScalar();
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
