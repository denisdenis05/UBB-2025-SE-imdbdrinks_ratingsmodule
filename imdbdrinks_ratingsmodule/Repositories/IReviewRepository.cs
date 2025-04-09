using System.Collections.Generic;
using imdbdrinks_ratingsmodule.Domain;

namespace imdbdrinks_ratingsmodule.Repositories
{
    public interface IReviewRepository
    {
        Review GetReviewById(int reviewId);
        IEnumerable<Review> GetAllReviews();
        IEnumerable<Review> GetReviewsByRatingId(int ratingId);
        Review AddOrUpdateReview(Review review);
        void DeleteReviewById(int reviewId);
    }
}
