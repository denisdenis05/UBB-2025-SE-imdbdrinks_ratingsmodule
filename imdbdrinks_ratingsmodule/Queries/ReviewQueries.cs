namespace imdbdrinks_ratingsmodule.Queries
{
    /// <summary>
    /// Contains SQL queries related to review operations.
    /// </summary>
    public static class ReviewQueries
    {
        /// <summary>
        /// The query to delete a review by its unique identifier.
        /// </summary>
        public const string DeleteReviewByIdQuery = "DELETE FROM Reviews WHERE ReviewId = @ReviewId";

        /// <summary>
        /// The query to retrieve all reviews.
        /// </summary>
        public const string GetAllReviewsQuery = "SELECT * FROM Reviews";

        /// <summary>
        /// The query to retrieve a review by its unique identifier.
        /// </summary>
        public const string GetReviewByIdQuery = "SELECT * FROM Reviews WHERE ReviewId = @ReviewId";

        /// <summary>
        /// The query to retrieve reviews associated with a specific rating identifier.
        /// </summary>
        public const string GetReviewsByRatingIdQuery = "SELECT * FROM Reviews WHERE RatingId = @RatingId";

        /// <summary>
        /// The query to insert a new review into the database.
        /// </summary>
        public const string AddReviewQuery =
            @"INSERT INTO Reviews (RatingId, UserId, Content, CreationDate, IsActive)
            OUTPUT INSERTED.ReviewId
            VALUES (@RatingId, @UserId, @Content, @CreationDate, @IsActive)";

        /// <summary>
        /// The query to update an existing review.
        /// </summary>
        public const string UpdateReviewQuery =
            @"UPDATE Reviews
            SET RatingId = @RatingId,
                UserId = @UserId,
                Content = @Content,
                CreationDate = @CreationDate,
                IsActive = @IsActive
            WHERE ReviewId = @ReviewId";

        /// <summary>
        /// The query to check if a review with the given identifier exists.
        /// </summary>
        public const string CheckIfIdExistsQuery =
            @"SELECT
                CASE
                    WHEN COUNT(*) = 1 THEN 1
                    ELSE 0
                END AS result
            FROM Reviews
            WHERE ReviewId = @ReviewId";
    }
}
