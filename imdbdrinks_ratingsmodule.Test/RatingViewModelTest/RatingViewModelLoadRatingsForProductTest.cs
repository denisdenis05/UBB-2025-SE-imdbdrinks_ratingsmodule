using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.Test.Helpers;
using imdbdrinks_ratingsmodule.ViewModels;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace imdbdrinks_ratingsmodule.Test.RatingViewModelTest
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
        public void LoadRatingsForProduct_NoRatings_ClearsRatingsCollection()
        {
            // Arrange
            var emptyRatings = new List<Rating>();
            
            RatingViewModelTestHelper.SetupGetRatingsByProduct(_mockRatingService, TestProductId, emptyRatings);
            RatingViewModelTestHelper.SetupGetAverageRating(_mockRatingService, TestProductId, 0);

            // Act
            _viewModel.LoadRatingsForProduct(TestProductId);

            // Assert
            Assert.That(_viewModel.Ratings, Is.Empty);
            Assert.That(_viewModel.AverageRating, Is.EqualTo(0));
        }

        [Test]
        public void LoadRatingsForProduct_WithRatings_LoadsRatingsInReverseOrder()
        {
            // Arrange
            var ratings = RatingViewModelTestHelper.CreateSampleRatings(TestProductId, 3);
            double expectedAverage = ratings.Average(r => r.RatingValue);
            
            RatingViewModelTestHelper.SetupGetRatingsByProduct(_mockRatingService, TestProductId, ratings);
            RatingViewModelTestHelper.SetupGetAverageRating(_mockRatingService, TestProductId, expectedAverage);

            // Act
            _viewModel.LoadRatingsForProduct(TestProductId);

            // Assert
            Assert.That(_viewModel.Ratings.Count, Is.EqualTo(3));
            
            // Verify ratings are in reverse order (newest first)
            Assert.That(_viewModel.Ratings.ElementAt(0).RatingId, Is.EqualTo(3));
            Assert.That(_viewModel.Ratings.ElementAt(1).RatingId, Is.EqualTo(2));
            Assert.That(_viewModel.Ratings.ElementAt(2).RatingId, Is.EqualTo(1));
            
            Assert.That(_viewModel.AverageRating, Is.EqualTo(expectedAverage));
        }

        [Test]
        public void LoadRatingsForProduct_WithRatings_UpdatesAverageRating()
        {
            // Arrange
            var ratings = RatingViewModelTestHelper.CreateSampleRatings(TestProductId, 2);
            const double expectedAverage = 3.5;
            
            RatingViewModelTestHelper.SetupGetRatingsByProduct(_mockRatingService, TestProductId, ratings);
            RatingViewModelTestHelper.SetupGetAverageRating(_mockRatingService, TestProductId, expectedAverage);

            // Act
            _viewModel.LoadRatingsForProduct(TestProductId);

            // Assert
            Assert.That(_viewModel.AverageRating, Is.EqualTo(expectedAverage));
        }

        [Test]
        public void LoadRatingsForProduct_CalledMultipleTimes_ClearsRatingsBeforeAdding()
        {
            // Arrange
            var firstCallRatings = RatingViewModelTestHelper.CreateSampleRatings(TestProductId, 2);
            var secondCallRatings = new List<Rating>
            {
                new Rating { RatingId = 3, ProductId = TestProductId, UserId = 3, RatingValue = 3, IsActive = true }
            };
            
            _mockRatingService.SetupSequence(service => service.GetRatingsByProduct(TestProductId))
                .Returns(firstCallRatings)
                .Returns(secondCallRatings);
            
            _mockRatingService.SetupSequence(service => service.GetAverageRating(TestProductId))
                .Returns(4.5)
                .Returns(3.0);

            // Act
            _viewModel.LoadRatingsForProduct(TestProductId); // First call
            _viewModel.LoadRatingsForProduct(TestProductId); // Second call

            // Assert
            Assert.That(_viewModel.Ratings.Count, Is.EqualTo(1)); // Only the second set remains
            Assert.That(_viewModel.Ratings.ElementAt(0).RatingId, Is.EqualTo(3));
            Assert.That(_viewModel.AverageRating, Is.EqualTo(3.0)); // Updated average
        }
        
        [Test]
        public void LoadRatingsForProduct_CallsServiceMethodsWithCorrectParameters()
        {
            // Arrange
            var ratings = RatingViewModelTestHelper.CreateSampleRatings(TestProductId, 3);
            
            _mockRatingService.Setup(service => service.GetRatingsByProduct(It.IsAny<int>()))
                .Returns(ratings);
            
            _mockRatingService.Setup(service => service.GetAverageRating(It.IsAny<int>()))
                .Returns(4.0);

            // Act
            _viewModel.LoadRatingsForProduct(TestProductId);

            // Assert
            _mockRatingService.Verify(service => service.GetRatingsByProduct(TestProductId), Times.Once);
            _mockRatingService.Verify(service => service.GetAverageRating(TestProductId), Times.Once);
        }
        
        [Test]
        public void LoadRatingsForProduct_FiltersRatingsByProductId()
        {
            // Arrange
            var productRatings = RatingViewModelTestHelper.CreateSampleRatings(TestProductId, 2);
            var otherProductRatings = RatingViewModelTestHelper.CreateSampleRatings(OtherProductId, 3);
            
            // Setup both product IDs with different ratings
            _mockRatingService.Setup(service => service.GetRatingsByProduct(TestProductId))
                .Returns(productRatings);
            
            _mockRatingService.Setup(service => service.GetRatingsByProduct(OtherProductId))
                .Returns(otherProductRatings);
                
            _mockRatingService.Setup(service => service.GetAverageRating(TestProductId))
                .Returns(3.5);
                
            _mockRatingService.Setup(service => service.GetAverageRating(OtherProductId))
                .Returns(4.0);

            // Act - load the first product
            _viewModel.LoadRatingsForProduct(TestProductId);
            
            // Assert - should have the first product's ratings
            Assert.That(_viewModel.Ratings.Count, Is.EqualTo(2));
            foreach (var rating in _viewModel.Ratings)
            {
                Assert.That(rating.ProductId, Is.EqualTo(TestProductId));
            }
            Assert.That(_viewModel.AverageRating, Is.EqualTo(3.5));
            
            // Act - load the second product
            _viewModel.LoadRatingsForProduct(OtherProductId);
            
            // Assert - should have switched to the second product's ratings
            Assert.That(_viewModel.Ratings.Count, Is.EqualTo(3));
            foreach (var rating in _viewModel.Ratings)
            {
                Assert.That(rating.ProductId, Is.EqualTo(OtherProductId));
            }
            Assert.That(_viewModel.AverageRating, Is.EqualTo(4.0));
        }
    }
} 