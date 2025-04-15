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
    public class ReviewViewModelAddReviewTest
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
        public void AddReview_WhenReviewContentIsNotEmpty_CallsAddReviewServiceMethod()
        {
            // Arrange
            const string reviewContent = "This is a test review";
            _viewModel.ReviewContent = reviewContent;
            var ratingId = 1;

            var reviewToAdd = new Review
            {
                RatingId = ratingId,
                UserId = 999,
                Content = reviewContent,
                IsActive = true
            };

            ReviewViewModelTestHelper.SetupAddReview(_mockReviewService, reviewToAdd);

            // Act
            _viewModel.AddReview(ratingId);

            // Assert
            _mockReviewService.Verify(service => service.AddReview(It.Is<Review>(review => review.Content == reviewContent)), Times.Once);
        }

        [Test]
        public void AddReview_WhenReviewContentIsEmpty_DoesNotCallAddReviewServiceMethod()
        {
            // Arrange
            _viewModel.ReviewContent = string.Empty;
            var ratingId = 1;

            // Act
            _viewModel.AddReview(ratingId);

            // Assert
            _mockReviewService.Verify(service => service.AddReview(It.IsAny<Review>()), Times.Never);
        }
    }
}
