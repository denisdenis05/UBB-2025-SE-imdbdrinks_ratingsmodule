using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.Test.Helpers;
using imdbdrinks_ratingsmodule.ViewModels;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace imdbdrinks_ratingsmodule.Test
{
    public class RatingViewModelLoadRatingsForProductTest
    {
        private RatingViewModel _viewModel;
        private Mock<IRatingService> _mockRatingService;
        private const int TestProductId = 100;
        private const int OtherProductId = 200;

        [SetUp]
        public void Setup()
        {
            _mockRatingService = RatingViewModelTestHelper.CreateMockRatingService();
            _viewModel = RatingViewModelTestHelper.CreateRatingViewModel(_mockRatingService);
        }

        [Test]
        public void LoadRatingsForProduct_NoRatings_RatingsCollectionIsEmpty()
        {
            _mockRatingService.Setup(service => service.GetRatingsByProduct(TestProductId))
                              .Returns(new List<Rating>());
            _mockRatingService.Setup(service => service.GetAverageRating(TestProductId)).Returns(0);

            _viewModel.LoadRatingsForProduct(TestProductId);

            Assert.That(_viewModel.Ratings, Is.Empty);
        }

        [Test]
        public void LoadRatingsForProduct_NoRatings_AverageRatingIsZero()
        {   
            var expectedAverage = 0;
            _mockRatingService.Setup(service => service.GetRatingsByProduct(TestProductId))
                              .Returns(new List<Rating>());
            _mockRatingService.Setup(service => service.GetAverageRating(TestProductId)).Returns(0);

            _viewModel.LoadRatingsForProduct(TestProductId);

            Assert.That(_viewModel.AverageRating, Is.EqualTo(expectedAverage));
        }

        [Test]
        public void LoadRatingsForProduct_WithRatings_LoadsExpectedCount()
        {
            var expectedCount = 3;
            var ratings = RatingViewModelTestHelper.CreateSampleRatings(TestProductId, 3);
            _mockRatingService.Setup(service => service.GetRatingsByProduct(TestProductId)).Returns(ratings);
            _mockRatingService.Setup(service => service.GetAverageRating(TestProductId)).Returns(4);

            _viewModel.LoadRatingsForProduct(TestProductId);

            Assert.That(_viewModel.Ratings.Count, Is.EqualTo(expectedCount));
        }

        [Test]
        public void LoadRatingsForProduct_WithRatings_OrdersByDescendingId()
        {
            var expectedOrder = new[] { 3, 2, 1 };
            var ratings = RatingViewModelTestHelper.CreateSampleRatings(TestProductId, 3);
            _mockRatingService.Setup(service => service.GetRatingsByProduct(TestProductId)).Returns(ratings);
            _mockRatingService.Setup(service => service.GetAverageRating(TestProductId)).Returns(4);

            _viewModel.LoadRatingsForProduct(TestProductId);

            Assert.That(_viewModel.Ratings.Select(rating => rating.RatingId), Is.EqualTo(expectedOrder));
        }

        [Test]
        public void LoadRatingsForProduct_WithRatings_SetsCorrectAverage()
        {
            var ratings = RatingViewModelTestHelper.CreateSampleRatings(TestProductId, 3);
            var expectedAverage = ratings.Average(r => r.RatingValue);

            _mockRatingService.Setup(service => service.GetRatingsByProduct(TestProductId)).Returns(ratings);
            _mockRatingService.Setup(service => service.GetAverageRating(TestProductId)).Returns(expectedAverage);

            _viewModel.LoadRatingsForProduct(TestProductId);

            Assert.That(_viewModel.AverageRating, Is.EqualTo(expectedAverage));
        }

        [Test]
        public void LoadRatingsForProduct_CallsGetRatingsByProduct_WithCorrectId()
        {
            _mockRatingService.Setup(service => service.GetRatingsByProduct(TestProductId)).Returns(new List<Rating>());
            _mockRatingService.Setup(service => service.GetAverageRating(TestProductId)).Returns(0);

            _viewModel.LoadRatingsForProduct(TestProductId);

            _mockRatingService.Verify(service => service.GetRatingsByProduct(TestProductId), Times.Once);
        }

        [Test]
        public void LoadRatingsForProduct_CallsGetAverageRating_WithCorrectId()
        {
            _mockRatingService.Setup(service => service.GetRatingsByProduct(TestProductId)).Returns(new List<Rating>());
            _mockRatingService.Setup(service => service.GetAverageRating(TestProductId)).Returns(0);

            _viewModel.LoadRatingsForProduct(TestProductId);

            _mockRatingService.Verify(service => service.GetAverageRating(TestProductId), Times.Once);
        }
    }
} 
