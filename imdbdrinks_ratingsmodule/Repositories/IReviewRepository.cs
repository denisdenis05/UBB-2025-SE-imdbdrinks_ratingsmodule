namespace imdbdrinks_ratingsmodule.Repositories
{
    using System.Collections.Generic;
    using imdbdrinks_ratingsmodule.Domain;

    public interface IReviewRepository
    {
        Review GetReviewById(int reviewId);

        IEnumerable<Review> GetAllReviews();

        IEnumerable<Review> GetReviewsByRatingId(int ratingId);

        public int AddReview(Review review);

        public Review UpdateReview(Review review);

        Review AddOrUpdateReview(Review review);

        void DeleteReviewById(int reviewId);

        bool CheckIfReviewWithIdExists(int reviewId);

    }
}
