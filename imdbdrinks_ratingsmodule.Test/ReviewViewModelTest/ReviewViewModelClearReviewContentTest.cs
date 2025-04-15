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
    public class ReviewViewModelClearReviewContentTest
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
        public void ClearReviewContent_WhenCalled_ClearsReviewContent()
        {
            // Arrange
            _viewModel.ReviewContent = "This is a test review";

            // Act
            _viewModel.ClearReviewContent();

            // Assert
            Assert.That(_viewModel.ReviewContent, Is.Empty);
        }
    }
}
