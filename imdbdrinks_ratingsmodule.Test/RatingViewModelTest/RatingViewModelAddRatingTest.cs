using imdbdrinks_ratingsmodule.Constants;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.Test.Helpers;
using imdbdrinks_ratingsmodule.ViewModels;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace imdbdrinks_ratingsmodule.Test
{
    public class RatingViewModelAddRatingTest
    {
        private RatingViewModel _viewModel;
        private Mock<IRatingService> _mockRatingService;
        private const int TestProductId = 100;

        [SetUp]
        public void Setup()
        {
            _mockRatingService = RatingViewModelTestHelper.CreateMockRatingService();
            _viewModel = RatingViewModelTestHelper.CreateRatingViewModel(_mockRatingService);
        }

        [Test]
        public void AddRating_WithValidScore_CallsCreateRating()
        {
            const int ratingScore = 4;
            _viewModel.RatingScore = ratingScore;

            _mockRatingService.Setup(service => service.CreateRating(It.IsAny<Rating>()))
                              .Returns(new Rating { ProductId = TestProductId });

            _viewModel.AddRating();

            _mockRatingService.Verify(service => service.CreateRating(It.IsAny<Rating>()), Times.Once);
        }

        [Test]
        public void AddRating_WithValidScore_CallsGetRatingsByProduct()
        {
            const int ratingScore = 4;
            _viewModel.RatingScore = ratingScore;

            _mockRatingService.Setup(service => service.CreateRating(It.IsAny<Rating>()))
                              .Returns(new Rating { ProductId = TestProductId });

            _viewModel.AddRating();

            _mockRatingService.Verify(service => service.GetRatingsByProduct(TestProductId), Times.Once);
        }

        [Test]
        public void AddRating_WithValidScore_CallsGetAverageRating()
        {
            const int ratingScore = 4;
            _viewModel.RatingScore = ratingScore;

            _mockRatingService.Setup(service => service.CreateRating(It.IsAny<Rating>()))
                              .Returns(new Rating { ProductId = TestProductId });

            _viewModel.AddRating();

            _mockRatingService.Verify(service => service.GetAverageRating(TestProductId), Times.Once);
        }

        [Test]
        public void AddRating_WithInvalidScore_DoesNotCallCreateRating()
        {
            const int invalidScore = 0;
            _viewModel.RatingScore = invalidScore;

            _viewModel.AddRating();

            _mockRatingService.Verify(service => service.CreateRating(It.IsAny<Rating>()), Times.Never);
        }

        [Test]
        public void AddRating_WithInvalidScore_DoesNotCallGetRatings()
        {
            const int invalidScore = 0;
            _viewModel.RatingScore = invalidScore;

            _viewModel.AddRating();

            _mockRatingService.Verify(service => service.GetRatingsByProduct(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddRating_AssignsCorrectUserId()
        {
            var existingRatings = RatingViewModelTestHelper.CreateSampleRatings(TestProductId, 3);
            _mockRatingService.Setup(service => service.GetRatingsByProduct(TestProductId)).Returns(existingRatings);

            Rating capturedRating = null;
            _mockRatingService.Setup(service => service.CreateRating(It.IsAny<Rating>()))
                              .Callback<Rating>(rating => capturedRating = rating)
                              .Returns((Rating rating) => rating);

            _viewModel.LoadRatingsForProduct(TestProductId);
            _viewModel.RatingScore = 5;
            var expectedUserId = 4;
            _viewModel.AddRating();

            Assert.That(capturedRating.UserId, Is.EqualTo(expectedUserId));
        }

        [Test]
        public void AddRating_AfterAdd_RatingsCollectionUpdated()
        {
            const int ratingScore = 3;
            var initialRatings = new List<Rating>();
            var updatedRatings = new List<Rating>{new Rating { RatingId = 1, ProductId = TestProductId, RatingValue = ratingScore, UserId = 1 }};
            var ratingsCount = updatedRatings.Count;
            _mockRatingService.SetupSequence(service => service.GetRatingsByProduct(TestProductId))
                              .Returns(initialRatings)
                              .Returns(updatedRatings);
            _mockRatingService.Setup(service => service.CreateRating(It.IsAny<Rating>())).Returns(updatedRatings[0]);
            _mockRatingService.Setup(service => service.GetAverageRating(TestProductId)).Returns(ratingScore);

            _viewModel.LoadRatingsForProduct(TestProductId);
            _viewModel.RatingScore = ratingScore;
            _viewModel.AddRating();

            Assert.That(_viewModel.Ratings.Count, Is.EqualTo(ratingsCount));
        }

        [Test]
        public void AddRating_SetsProductIdToPlaceholder()
        {
            const int ratingScore = 4;
            const int expectedProductId = 100;

            Rating capturedRating = null;
            _mockRatingService.Setup(service => service.CreateRating(It.IsAny<Rating>()))
                              .Callback<Rating>(rating => capturedRating = rating)
                              .Returns((Rating rating) => rating);

            _viewModel.RatingScore = ratingScore;
            _viewModel.AddRating();

            Assert.That(capturedRating.ProductId, Is.EqualTo(expectedProductId));
        }
    }
} 
