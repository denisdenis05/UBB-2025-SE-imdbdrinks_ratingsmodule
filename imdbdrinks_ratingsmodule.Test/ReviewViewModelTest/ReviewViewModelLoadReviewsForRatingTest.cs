using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Test.Helpers;
using imdbdrinks_ratingsmodule.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imdbdrinks_ratingsmodule.Test
{
    public class ReviewViewModelLoadReviewsForRatingTest
    {
        private Mock<IReviewService> _mockReviewService;
        private ReviewViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _mockReviewService = ReviewViewModelTestHelper.CreateMockReviewService();
            _viewModel = ReviewViewModelTestHelper.CreateReviewViewModel(_mockReviewService);
        }

        [Test]
        public void LoadReviewsForRating_WhenCalled_LoadsReviewsIntoViewModel()
        {
            // Arrange
            var ratingId = 1;
            var sampleReviews = ReviewViewModelTestHelper.CreateSampleReviews(ratingId);

            ReviewViewModelTestHelper.SetupGetReviewsByRating(_mockReviewService, ratingId, sampleReviews);

            // Act
            _viewModel.LoadReviewsForRating(ratingId);

            // Assert
            Assert.That(_viewModel.Reviews.Count, Is.EqualTo(sampleReviews.Count));
        }

        [Test]
        public void LoadReviewsForRating_WhenReviewsAreLoaded_ClearsExistingReviews()
        {
            // Arrange
            var ratingId = 1;
            var reviewId = 999;
            var sampleReviews = ReviewViewModelTestHelper.CreateSampleReviews(ratingId);
            _viewModel.Reviews.Add(new Review { ReviewId = reviewId, Content = "Existing review" });

            ReviewViewModelTestHelper.SetupGetReviewsByRating(_mockReviewService, ratingId, sampleReviews);

            // Act
            _viewModel.LoadReviewsForRating(ratingId);

            // Assert
            Assert.That(_viewModel.Reviews.Count, Is.EqualTo(sampleReviews.Count));
            Assert.That(_viewModel.Reviews, Does.Not.Contain(new Review { ReviewId = reviewId }));
        }
    }
}
