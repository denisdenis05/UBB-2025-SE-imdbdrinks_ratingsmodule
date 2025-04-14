namespace imdbdrinks_ratingsmodule.Queries
{
    public static class RatingQueries
    {
        public const string GetAllRatingsQuery = "SELECT * FROM Ratings";

        public const string GetRatingByIdQuery = "SELECT * FROM Ratings WHERE RatingId = @RatingId";

        public const string GetRatingsByProductIdQuery = "SELECT * FROM Ratings WHERE ProductId = @ProductId";

        public const string AddRatingQuery =
            @"INSERT INTO Ratings (ProductId, UserId, RatingValue, RatingDate, IsActive)
            OUTPUT INSERTED.RatingId
            VALUES (@ProductId, @UserId, @RatingValue, @RatingDate, @IsActive)";

        public const string UpdateRatingQuery =
            @"UPDATE Ratings
            SET ProductId = @ProductId,
                UserId = @UserId,
                RatingValue = @RatingValue,
                RatingDate = @RatingDate,
                IsActive = @IsActive
            WHERE RatingId = @RatingId";

        public const string DeleteRatingQuery = "DELETE FROM Ratings WHERE RatingId = @RatingId";

        public const string CheckIfRatingWithIdExistsQuery =
            @"SELECT
                CASE
                    WHEN COUNT(*) = 1 THEN 1
                    ELSE 0
                END AS result
            FROM Ratings
            WHERE RatingId = @RatingId";

        public const string GetAverageRatingQuery = "SELECT AVG(RatingValue) FROM Ratings WHERE ProductId = @ProductId";

        public const string GetRatingCountQuery = "SELECT COUNT(*) FROM Ratings WHERE ProductId = @ProductId";
    }
}
