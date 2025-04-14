using imdbdrinks_ratingsmodule.Constants;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.Test.Helpers;
using imdbdrinks_ratingsmodule.ViewModels;
using imdbdrinks_ratingsmodule.ViewHelpers;
using Moq;
using System.Linq;

namespace imdbdrinks_ratingsmodule.Test.RatingViewModelTest
{
    public class RatingViewModelUpdateBottleRatingTest
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
        public void UpdateBottleRating_SetsRatingScore()
        {
            // Arrange
            const int clickedBottleNumber = 3;

            // Act
            _viewModel.UpdateBottleRating(clickedBottleNumber);

            // Assert
            Assert.That(_viewModel.RatingScore, Is.EqualTo(clickedBottleNumber));
        }

        [Test]
        public void UpdateBottleRating_UpdatesAllBottlesCorrectly()
        {
            // Arrange
            const int clickedBottleNumber = 3;
            
            // Act
            _viewModel.UpdateBottleRating(clickedBottleNumber);

            // Assert
            for (int i = 0; i < RatingDomainConstants.MaxRatingValue; i++)
            {
                int bottleRating = i + 1; // Convert from 0-based index to 1-based rating
                
                if (bottleRating <= clickedBottleNumber)
                {
                    // Bottles up to and including clicked bottle should be filled
                    Assert.That(_viewModel.Bottles[i].ImageSource, Is.EqualTo(AssetConstants.FilledBottlePath));
                }
                else
                {
                    // Bottles after clicked bottle should be empty
                    Assert.That(_viewModel.Bottles[i].ImageSource, Is.EqualTo(AssetConstants.EmptyBottlePath));
                }
            }
        }

        [Test]
        public void UpdateBottleRating_WithMinimumValue_UpdatesBottlesCorrectly()
        {
            // Arrange
            const int clickedBottleNumber = RatingDomainConstants.MinRatingValue;
            
            // Act
            _viewModel.UpdateBottleRating(clickedBottleNumber);

            // Assert
            // Only the first bottle should be filled
            Assert.That(_viewModel.Bottles[0].ImageSource, Is.EqualTo(AssetConstants.FilledBottlePath));
            
            // The rest should be empty
            for (int i = 1; i < RatingDomainConstants.MaxRatingValue; i++)
            {
                Assert.That(_viewModel.Bottles[i].ImageSource, Is.EqualTo(AssetConstants.EmptyBottlePath));
            }
            
            Assert.That(_viewModel.RatingScore, Is.EqualTo(clickedBottleNumber));
        }

        [Test]
        public void UpdateBottleRating_WithMaximumValue_UpdatesBottlesCorrectly()
        {
            // Arrange
            const int clickedBottleNumber = RatingDomainConstants.MaxRatingValue;
            
            // Act
            _viewModel.UpdateBottleRating(clickedBottleNumber);

            // Assert
            // All bottles should be filled
            foreach (var bottle in _viewModel.Bottles)
            {
                Assert.That(bottle.ImageSource, Is.EqualTo(AssetConstants.FilledBottlePath));
            }
            
            Assert.That(_viewModel.RatingScore, Is.EqualTo(clickedBottleNumber));
        }

        [Test]
        public void UpdateBottleRating_CalledMultipleTimes_UpdatesBottlesCorrectly()
        {
            // First click on bottle 4
            _viewModel.UpdateBottleRating(4);
            
            // Assert all bottles up to 4 are filled
            for (int i = 0; i < 4; i++)
            {
                Assert.That(_viewModel.Bottles[i].ImageSource, Is.EqualTo(AssetConstants.FilledBottlePath));
            }
            
            // Then click on bottle 2
            _viewModel.UpdateBottleRating(2);
            
            // Assert only bottles 1 and 2 are filled, others are empty
            Assert.That(_viewModel.Bottles[0].ImageSource, Is.EqualTo(AssetConstants.FilledBottlePath));
            Assert.That(_viewModel.Bottles[1].ImageSource, Is.EqualTo(AssetConstants.FilledBottlePath));
            Assert.That(_viewModel.Bottles[2].ImageSource, Is.EqualTo(AssetConstants.EmptyBottlePath));
            Assert.That(_viewModel.Bottles[3].ImageSource, Is.EqualTo(AssetConstants.EmptyBottlePath));
            Assert.That(_viewModel.Bottles[4].ImageSource, Is.EqualTo(AssetConstants.EmptyBottlePath));
            
            Assert.That(_viewModel.RatingScore, Is.EqualTo(2));
        }

        [Test]
        public void InitializeBottles_CreatesCorrectNumberOfBottles()
        {
            // The InitializeBottles method is called in the constructor
            // so we can just verify the bottles collection has the correct number of items
            
            // Assert
            Assert.That(_viewModel.Bottles.Count, Is.EqualTo(RatingDomainConstants.MaxRatingValue));
            
            // All bottles should start empty
            foreach (var bottle in _viewModel.Bottles)
            {
                Assert.That(bottle.ImageSource, Is.EqualTo(AssetConstants.EmptyBottlePath));
            }
        }
    }
} 