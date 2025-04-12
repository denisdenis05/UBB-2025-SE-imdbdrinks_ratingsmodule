namespace imdbdrinks_ratingsmodule.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using imdbdrinks_ratingsmodule.Domain;
    using Microsoft.Data.SqlClient;
    using Microsoft.UI.Xaml.Controls.Primitives;

    public static class ReviewQueries
    {
        public const string DeleteReviewByIdQuery = "DELETE FROM Reviews WHERE ReviewId = @ReviewId";

        public const string GetAllReviewsQuery = "SELECT * FROM Reviews";

        public const string GetReviewByIdQuery = "SELECT * FROM Reviews WHERE ReviewId = @ReviewId";

        public const string GetReviewsByRatingIdQuery = "SELECT * FROM Reviews WHERE RatingId = @RatingId";

        public const string AddReviewQuery =
                        @"INSERT INTO Reviews (RatingId, UserId, Content, CreationDate, IsActive)
                        OUTPUT INSERTED.ReviewId
                        VALUES (@RatingId, @UserId, @Content, @CreationDate, @IsActive)";

        public const string UpdateReviewQuery =
                        @"UPDATE Reviews
                        SET RatingId = @RatingId,
                            UserId = @UserId,
                            Content = @Content,
                            CreationDate = @CreationDate,
                            IsActive = @IsActive
                        WHERE ReviewId = @ReviewId";

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
