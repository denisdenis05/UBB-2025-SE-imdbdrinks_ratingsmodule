namespace imdbdrinks_ratingsmodule.Queries
{
    /// <summary>
    /// Contains the SQL queries related to ratings operations.
    /// </summary>
    public static class RatingQueries
    {
        /// <summary>
        /// The query to retrieve all ratings.
        /// </summary>
        public const string GetAllRatingsQuery = "SELECT * FROM Ratings";

        /// <summary>
        /// The query to retrieve a rating by its unique identifier.
        /// </summary>
        public const string GetRatingByIdQuery = "SELECT * FROM Ratings WHERE RatingId = @RatingId";

        /// <summary>
        /// The query to retrieve ratings by product identifier.
        /// </summary>
        public const string GetRatingsByProductIdQuery = "SELECT * FROM Ratings WHERE ProductId = @ProductId";

        /// <summary>
        /// The query to insert a new rating into the database.
        /// </summary>
        public const string AddRatingQuery =
            @"INSERT INTO Ratings (ProductId, UserId, RatingValue, RatingDate, IsActive)
            OUTPUT INSERTED.RatingId
            VALUES (@ProductId, @UserId, @RatingValue, @RatingDate, @IsActive)";

        /// <summary>
        /// The query to update an existing rating.
        /// </summary>
        public const string UpdateRatingQuery =
            @"UPDATE Ratings
            SET ProductId = @ProductId,
                UserId = @UserId,
                RatingValue = @RatingValue,
                RatingDate = @RatingDate,
                IsActive = @IsActive
            WHERE RatingId = @RatingId";

        /// <summary>
        /// The query to delete a rating by its unique identifier.
        /// </summary>
        public const string DeleteRatingQuery = "DELETE FROM Ratings WHERE RatingId = @RatingId";

        /// <summary>
        /// The query to check if a rating with the given identifier exists.
        /// </summary>
        public const string CheckIfRatingWithIdExistsQuery =
            @"SELECT
                CASE
                    WHEN COUNT(*) = 1 THEN 1
                    ELSE 0
                END AS result
            FROM Ratings
            WHERE RatingId = @RatingId";

        /// <summary>
        /// The query to get the average rating for a product.
        /// </summary>
        public const string GetAverageRatingQuery = "SELECT AVG(RatingValue) FROM Ratings WHERE ProductId = @ProductId";

        /// <summary>
        /// The query to get the total number of ratings for a product.
        /// </summary>
        public const string GetRatingCountQuery = "SELECT COUNT(*) FROM Ratings WHERE ProductId = @ProductId";
    }
}
