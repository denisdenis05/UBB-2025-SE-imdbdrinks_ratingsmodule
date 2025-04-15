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
    public class RatingViewModelSelectedRatingTest
    {
        private RatingViewModel _viewModel;
        private Mock<IRatingService> _mockRatingService;

        [SetUp]
        public void Setup()
        {
            _mockRatingService = RatingViewModelTestHelper.CreateMockRatingService();
            _viewModel = RatingViewModelTestHelper.CreateRatingViewModel(_mockRatingService);
        }

        [Test]
        public void SelectedRating_WhenSet_CanBeRetrieved()
        {
            // Arrange
            var testRating = new Rating
            {
                RatingId = 1,
                ProductId = 101,
                RatingValue = 5,
                UserId = 7
            };

            // Act
            _viewModel.SelectedRating = testRating;

            // Assert
            Assert.That(_viewModel.SelectedRating, Is.EqualTo(testRating));
        }

        [Test]
        public void SelectedRating_WhenUnset_IsNullByDefault()
        {
            Assert.That(_viewModel.SelectedRating, Is.Null);
        }
    }
}
