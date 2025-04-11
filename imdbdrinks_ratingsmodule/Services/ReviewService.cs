using System;
using System.Collections.Generic;
using System.Linq;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;

namespace imdbdrinks_ratingsmodule.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public IEnumerable<Review> GetReviewsByRating(int ratingId) =>
            _reviewRepository.GetReviewsByRatingId(ratingId);

        public Review CreateReview(Review review)
        {
            if (!review.IsValid())
                throw new ArgumentException("Invalid review content.");

            review.CreationDate = DateTime.Now;
            review.IsActive = true;
            return _reviewRepository.AddOrUpdateReview(review);
        }

        public void DeleteReview(int reviewId) =>
            _reviewRepository.DeleteReviewById(reviewId);
    }
}
