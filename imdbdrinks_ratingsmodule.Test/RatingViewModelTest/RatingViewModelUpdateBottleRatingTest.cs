using imdbdrinks_ratingsmodule.Constants;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.Test.Helpers;
using imdbdrinks_ratingsmodule.ViewModels;
using imdbdrinks_ratingsmodule.ViewHelpers;
using Moq;
using System.Linq;

namespace imdbdrinks_ratingsmodule.Test
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
        public void UpdateBottleRating_WhenClicked_SetsRatingScore()
        {
            const int clickedBottleNumber = 3;

            _viewModel.UpdateBottleRating(clickedBottleNumber);

            Assert.That(_viewModel.RatingScore, Is.EqualTo(clickedBottleNumber));
        }

        [Test]
        public void UpdateBottleRating_MinimumValue_FillsOnlyFirstBottle()
        {
            const int clickedBottleNumber = RatingDomainConstants.MinRatingValue;

            _viewModel.UpdateBottleRating(clickedBottleNumber);

            Assert.That(_viewModel.Bottles[0].ImageSource, Is.EqualTo(AssetConstants.FilledBottlePath));
        }

        [Test]
        public void UpdateBottleRating_MaximumValue_SetsRatingScore()
        {
            const int clickedBottleNumber = RatingDomainConstants.MaxRatingValue;

            _viewModel.UpdateBottleRating(clickedBottleNumber);

            Assert.That(_viewModel.RatingScore, Is.EqualTo(clickedBottleNumber));
        }


        [Test]
        public void InitializeBottles_CreatesCorrectNumberOfBottles()
        {
            Assert.That(_viewModel.Bottles.Count, Is.EqualTo(RatingDomainConstants.MaxRatingValue));
        }


    }

}