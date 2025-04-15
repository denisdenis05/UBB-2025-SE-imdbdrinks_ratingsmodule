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
    public class ReviewViewModelSelectedReviewTest
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
        public void SelectedReview_WhenSet_UpdatesProperty()
        {
            // Arrange
            var review = new Review { ReviewId = 1, Content = "Test Review" };

            // Act
            _viewModel.SelectedReview = review;

            // Assert
            Assert.That(_viewModel.SelectedReview, Is.EqualTo(review));
        }
    }
}
