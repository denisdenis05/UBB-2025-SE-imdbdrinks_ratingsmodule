using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imdbdrinks_ratingsmodule.Test.Helpers
{
    public interface IReviewService
    {
        IEnumerable<Review> GetReviewsByRating(int ratingId);
        Review AddReview(Review review);
        void DeleteReviewById(int reviewId);
    }

    public static class ReviewViewModelTestHelper
    {
        public static Mock<IReviewService> CreateMockReviewService()
        {
            return new Mock<IReviewService>();
        }

        public static ReviewViewModel CreateReviewViewModel(Mock<IReviewService> mockReviewService = null)
        {
            mockReviewService ??= CreateMockReviewService();
            // We need to adapt our mocked interface to work with the ReviewViewModel that expects a ReviewService
            var mockReviewServiceWrapper = new ReviewServiceTestWrapper(mockReviewService.Object);
            return new ReviewViewModel(mockReviewServiceWrapper);
        }

        public static List<Review> CreateSampleReviews(int ratingId, int count = 3)
        {
            var reviews = new List<Review>();

            for (int i = 1; i <= count; i++)
            {
                reviews.Add(new Review
                {
                    ReviewId = i,
                    RatingId = ratingId,
                    UserId = i,
                    Content = "Review " + i,
                    IsActive = true,
                    CreationDate = System.DateTime.Now.AddDays(-i) // Older reviews have earlier dates
                });
            }

            return reviews;
        }

        public static void SetupGetReviewsByRating(
            Mock<IReviewService> mockService,
            int ratingId,
            List<Review> reviews)
        {
            mockService.Setup(service => service.GetReviewsByRating(ratingId))
                .Returns(reviews);
        }

        public static void SetupAddReview(
            Mock<IReviewService> mockService,
            Review review)
        {
            mockService.Setup(service => service.AddReview(review))
                .Returns(review);
        }

        public static void SetupDeleteReviewById(
            Mock<IReviewService> mockService,
            int reviewId)
        {
            mockService.Setup(service => service.DeleteReviewById(reviewId))
                .Verifiable();
        }
    }

    // Wrapper class that implements ReviewService's functionality using our mockable interface
    public class ReviewServiceTestWrapper : ReviewService
    {
        private readonly IReviewService _mockReviewService;

        public ReviewServiceTestWrapper(IReviewService mockReviewService)
            : base(new Mock<IReviewRepository>().Object) // Pass a dummy repository to the base constructor
        {
            _mockReviewService = mockReviewService;
        }

        public override IEnumerable<Review> GetReviewsByRating(int ratingId)
            => _mockReviewService.GetReviewsByRating(ratingId);

        public override Review AddReview(Review review)
            => _mockReviewService.AddReview(review);

        public override void DeleteReviewById(int reviewId)
            => _mockReviewService.DeleteReviewById(reviewId);
    }
}
