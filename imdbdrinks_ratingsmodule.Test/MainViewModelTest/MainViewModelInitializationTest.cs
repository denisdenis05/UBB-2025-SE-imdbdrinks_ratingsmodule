using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.Test.Helpers;
using imdbdrinks_ratingsmodule.ViewModels;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.ObjectModel;
using System.Linq;

namespace imdbdrinks_ratingsmodule.Test
{
    public class MainViewModelInitializationTest
    {
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<RatingViewModel> _mockRatingViewModel;
        private Mock<ReviewViewModel> _mockReviewViewModel;
        private MainViewModel _viewModel;
        private const int DefaultProductId = 100;

        [SetUp]
        public void Setup()
        {
            _mockConfiguration = MainViewModelTestHelper.CreateMockConfiguration();
            _mockRatingViewModel = MainViewModelTestHelper.CreateMockRatingViewModel();
            _mockReviewViewModel = MainViewModelTestHelper.CreateMockReviewViewModel();
        }

        [Test]
        public void Constructor_InitializesProperties()
        {
            // Act
            _viewModel = MainViewModelTestHelper.CreateMainViewModel(
                _mockConfiguration,
                _mockRatingViewModel,
                _mockReviewViewModel);

            // Assert
            Assert.That(_viewModel.Configuration, Is.EqualTo(_mockConfiguration.Object));
            Assert.That(_viewModel.RatingViewModel, Is.EqualTo(_mockRatingViewModel.Object));
            Assert.That(_viewModel.ReviewViewModel, Is.EqualTo(_mockReviewViewModel.Object));
        }

        [Test]
        public void Constructor_CallsLoadRatingsForProduct()
        {
            // Arrange - Implement a tracking flag in setup
            bool loadRatingsWasCalled = false;
            int productIdPassed = 0;
            
            _mockRatingViewModel.Setup(vm => vm.LoadRatingsForProduct(It.IsAny<int>()))
                .Callback<int>(productId => {
                    loadRatingsWasCalled = true;
                    productIdPassed = productId;
                });

            // Act
            _viewModel = MainViewModelTestHelper.CreateMainViewModel(
                _mockConfiguration,
                _mockRatingViewModel,
                _mockReviewViewModel);

            // Assert
            Assert.That(loadRatingsWasCalled, Is.True, "LoadRatingsForProduct was not called");
            Assert.That(productIdPassed, Is.EqualTo(DefaultProductId), "Incorrect product ID was passed");
        }

        [Test]
        public void SelectedRating_ReturnsRatingFromRatingViewModel()
        {
            // Arrange
            var expectedRating = new Rating { RatingId = 1, ProductId = 100, RatingValue = 4 };
            _mockRatingViewModel.Setup(vm => vm.SelectedRating).Returns(expectedRating);
            
            _viewModel = MainViewModelTestHelper.CreateMainViewModel(
                _mockConfiguration,
                _mockRatingViewModel,
                _mockReviewViewModel);

            // Act
            var result = _viewModel.SelectedRating;

            // Assert
            Assert.That(result, Is.EqualTo(expectedRating));
        }

        [Test]
        public void RatingViewModel_SetProperty_NotifiesPropertyChanged()
        {
            // Arrange
            _viewModel = MainViewModelTestHelper.CreateMainViewModel(
                _mockConfiguration,
                _mockRatingViewModel,
                _mockReviewViewModel);

            bool propertyChangedFired = false;
            string propertyChangedName = null;

            _viewModel.PropertyChanged += (sender, args) =>
            {
                propertyChangedFired = true;
                propertyChangedName = args.PropertyName;
            };

            // Create a new mock to set
            var newRatingViewModel = MainViewModelTestHelper.CreateMockRatingViewModel();

            // Act
            _viewModel.RatingViewModel = newRatingViewModel.Object;

            // Assert
            Assert.That(propertyChangedFired, Is.True);
            Assert.That(propertyChangedName, Is.EqualTo(nameof(MainViewModel.RatingViewModel)));
            Assert.That(_viewModel.RatingViewModel, Is.EqualTo(newRatingViewModel.Object));
        }

        [Test]
        public void ReviewViewModel_SetProperty_NotifiesPropertyChanged()
        {
            // Arrange
            _viewModel = MainViewModelTestHelper.CreateMainViewModel(
                _mockConfiguration,
                _mockRatingViewModel,
                _mockReviewViewModel);

            bool propertyChangedFired = false;
            string propertyChangedName = null;

            _viewModel.PropertyChanged += (sender, args) =>
            {
                propertyChangedFired = true;
                propertyChangedName = args.PropertyName;
            };

            // Create a new mock to set
            var newReviewViewModel = MainViewModelTestHelper.CreateMockReviewViewModel();

            // Act
            _viewModel.ReviewViewModel = newReviewViewModel.Object;

            // Assert
            Assert.That(propertyChangedFired, Is.True);
            Assert.That(propertyChangedName, Is.EqualTo(nameof(MainViewModel.ReviewViewModel)));
            Assert.That(_viewModel.ReviewViewModel, Is.EqualTo(newReviewViewModel.Object));
        }
    }
} 