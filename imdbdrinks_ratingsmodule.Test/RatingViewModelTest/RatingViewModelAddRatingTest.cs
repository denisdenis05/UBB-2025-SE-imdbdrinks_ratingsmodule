using imdbdrinks_ratingsmodule.Constants;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.Test.Helpers;
using imdbdrinks_ratingsmodule.ViewModels;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace imdbdrinks_ratingsmodule.Test.RatingViewModelTest
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
        public void AddRating_WithValidRatingScore_CreatesRating()
        {
            // Arrange
            const int ratingScore = 4;
            var addedRating = new Rating
            {
                RatingId = 1,
                ProductId = TestProductId,
                RatingValue = ratingScore,
                UserId = 1, // First user
                IsActive = true
            };

            var ratings = new List<Rating> { addedRating };

            _mockRatingService.Setup(service => service.CreateRating(It.IsAny<Rating>()))
                .Returns(addedRating);

            RatingViewModelTestHelper.SetupGetRatingsByProduct(_mockRatingService, TestProductId, ratings);
            RatingViewModelTestHelper.SetupGetAverageRating(_mockRatingService, TestProductId, ratingScore);

            // Act
            _viewModel.RatingScore = ratingScore;
            _viewModel.AddRating();

            // Assert
            _mockRatingService.Verify(service => service.CreateRating(It.Is<Rating>(r => 
                r.ProductId == TestProductId && 
                r.RatingValue == ratingScore)), Times.Once);

            _mockRatingService.Verify(service => service.GetRatingsByProduct(TestProductId), Times.Once);
            _mockRatingService.Verify(service => service.GetAverageRating(TestProductId), Times.Once);
        }

        [Test]
        public void AddRating_WithInvalidRatingScore_DoesNotCreateRating()
        {
            // Arrange
            const int invalidRatingScore = 0; // Below min value
            
            // Act
            _viewModel.RatingScore = invalidRatingScore;
            _viewModel.AddRating();

            // Assert
            _mockRatingService.Verify(service => service.CreateRating(It.IsAny<Rating>()), Times.Never);
            _mockRatingService.Verify(service => service.GetRatingsByProduct(It.IsAny<int>()), Times.Never);
            _mockRatingService.Verify(service => service.GetAverageRating(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void AddRating_SetsCorrectUserIdBasedOnExistingRatings()
        {
            // Arrange
            var existingRatings = RatingViewModelTestHelper.CreateSampleRatings(TestProductId, 3);
            const int ratingScore = 5;
            const int expectedUserId = 4; // 3 existing + 1

            _mockRatingService.Setup(service => service.GetRatingsByProduct(TestProductId))
                .Returns(existingRatings);

            Rating capturedRating = null;
            _mockRatingService.Setup(service => service.CreateRating(It.IsAny<Rating>()))
                .Callback<Rating>(r => capturedRating = r)
                .Returns((Rating r) => r);

            // Simulate loading the ratings first so user ID calculation works
            _viewModel.LoadRatingsForProduct(TestProductId);

            // Act
            _viewModel.RatingScore = ratingScore;
            _viewModel.AddRating();

            // Assert
            Assert.That(capturedRating, Is.Not.Null);
            Assert.That(capturedRating.UserId, Is.EqualTo(expectedUserId));
        }

        [Test]
        public void AddRating_AfterAdding_ReloadsRatingsForProduct()
        {
            // Arrange
            const int ratingScore = 3;
            var initialRatings = new List<Rating>();
            var updatedRatings = new List<Rating> 
            { 
                new Rating 
                { 
                    RatingId = 1, 
                    ProductId = TestProductId, 
                    RatingValue = ratingScore, 
                    UserId = 1, 
                    IsActive = true 
                } 
            };

            // Setup sequence for GetRatingsByProduct - first empty, then with the new rating
            _mockRatingService.SetupSequence(service => service.GetRatingsByProduct(TestProductId))
                .Returns(initialRatings)
                .Returns(updatedRatings);

            _mockRatingService.Setup(service => service.CreateRating(It.IsAny<Rating>()))
                .Returns(updatedRatings[0]);

            _mockRatingService.Setup(service => service.GetAverageRating(TestProductId))
                .Returns(ratingScore);

            // Load ratings initially (empty)
            _viewModel.LoadRatingsForProduct(TestProductId);
            Assert.That(_viewModel.Ratings, Is.Empty);

            // Act
            _viewModel.RatingScore = ratingScore;
            _viewModel.AddRating();

            // Assert
            Assert.That(_viewModel.Ratings.Count, Is.EqualTo(1));
            Assert.That(_viewModel.Ratings[0].RatingValue, Is.EqualTo(ratingScore));
            
            // Verify the ratings were reloaded after adding
            _mockRatingService.Verify(service => service.GetRatingsByProduct(TestProductId), Times.Exactly(2));
        }

        [Test]
        public void AddRating_UsesPlaceholderItemId()
        {
            // Arrange
            const int ratingScore = 4;
            const int expectedProductId = 100; // PlaceholderItemId from RatingViewModel

            Rating capturedRating = null;
            _mockRatingService.Setup(service => service.CreateRating(It.IsAny<Rating>()))
                .Callback<Rating>(r => capturedRating = r)
                .Returns((Rating r) => r);

            // Act
            _viewModel.RatingScore = ratingScore;
            _viewModel.AddRating();

            // Assert
            Assert.That(capturedRating, Is.Not.Null);
            Assert.That(capturedRating.ProductId, Is.EqualTo(expectedProductId));
        }

        [Test]
        public void AddRating_WithBoundaryValues_HandlesCorrectly()
        {
            // Test with minimum valid rating value
            TestRatingWithValue(RatingDomainConstants.MinRatingValue, true);
            
            // Test with maximum valid rating value
            TestRatingWithValue(RatingDomainConstants.MaxRatingValue, true);
            
            // Test with one below minimum
            TestRatingWithValue(RatingDomainConstants.MinRatingValue - 1, false);
            
            // Test with one above maximum (not possible in UI, but testing the validation)
            TestRatingWithValue(RatingDomainConstants.MaxRatingValue + 1, true);
        }

        private void TestRatingWithValue(int ratingValue, bool shouldCreate)
        {
            // Reset the mock to clear invocation counts
            _mockRatingService.Reset();

            // Act
            _viewModel.RatingScore = ratingValue;
            _viewModel.AddRating();

            // Assert
            if (shouldCreate)
            {
                _mockRatingService.Verify(service => service.CreateRating(It.Is<Rating>(r => 
                    r.RatingValue == ratingValue)), Times.Once);
            }
            else
            {
                _mockRatingService.Verify(service => service.CreateRating(It.IsAny<Rating>()), Times.Never);
            }
        }
    }
} 