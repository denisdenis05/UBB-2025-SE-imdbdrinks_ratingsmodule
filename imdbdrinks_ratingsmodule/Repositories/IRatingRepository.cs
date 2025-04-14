using System.Collections.Generic;
using imdbdrinks_ratingsmodule.Domain;

namespace imdbdrinks_ratingsmodule.Repositories
{
    public interface IRatingRepository
    {
        Rating GetRatingById(int ratingId);

        IEnumerable<Rating> GetAllRatings();

        IEnumerable<Rating> GetRatingsByProductId(int productId);

        Rating AddRating(Rating rating);

        Rating UpdateRating(Rating rating);

        void DeleteRatingById(int ratingId);

        Rating AddOrUpdateRating(Rating rating);
    }
}
