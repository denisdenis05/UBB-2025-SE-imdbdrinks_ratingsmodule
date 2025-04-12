namespace imdbdrinks_ratingsmodule.Services
{
    using System;
    using System.Collections.Generic;
    using imdbdrinks_ratingsmodule.Constants.ErrorMessages;
    using imdbdrinks_ratingsmodule.Domain;
    using imdbdrinks_ratingsmodule.Repositories;

    public class ReviewService(IReviewRepository reviewRepository)
    {
        private readonly IReviewRepository reviewRepository = reviewRepository;

        public IEnumerable<Review> GetReviewsByRating(int ratingId)
        {
            return this.reviewRepository.GetReviewsByRatingId(ratingId);
        }

        public Review AddReview(Review review)
        {
            if (!review.IsValid())
            {
                throw new ArgumentException(ReviewServiceErrorMessages.InvalidReview);
            }

            review.Activate();
            return this.reviewRepository.AddOrUpdateReview(review);
        }

        public void DeleteReviewById(int reviewId)
        {
            this.reviewRepository.DeleteReviewById(reviewId);
        }
    }
}
