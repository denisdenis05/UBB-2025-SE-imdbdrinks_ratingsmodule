using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Test.Helpers;
using imdbdrinks_ratingsmodule.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Moq;
using System.Collections.ObjectModel;

namespace imdbdrinks_ratingsmodule.Test.MainViewModelTest
{
    public class MainViewModelHandleRatingSelectionTest
    {
        private MainViewModel _viewModel;
        private Mock<RatingViewModel> _mockRatingViewModel;
        private Mock<ReviewViewModel> _mockReviewViewModel;
        private ObservableCollection<Rating> _ratings;

        [SetUp]
        public void Setup()
        {
            _mockRatingViewModel = MainViewModelTestHelper.CreateMockRatingViewModel();
            _mockReviewViewModel = MainViewModelTestHelper.CreateMockReviewViewModel();
            
            // Create some sample ratings
            _ratings = new ObservableCollection<Rating>
            {
                new Rating { RatingId = 1, ProductId = 100, RatingValue = 4 },
                new Rating { RatingId = 2, ProductId = 100, RatingValue = 5 },
                new Rating { RatingId = 3, ProductId = 100, RatingValue = 3 }
            };
            
            // Set the Ratings property directly on the mocked object instead of using Setup
            _mockRatingViewModel.Object.Ratings = _ratings;
            
            _viewModel = MainViewModelTestHelper.CreateMainViewModel(
                mockRatingViewModel: _mockRatingViewModel, 
                mockReviewViewModel: _mockReviewViewModel);
        }

        [Test]
        public void HandleRatingSelection_WithValidIndex_SetsSelectedRating()
        {
            // Arrange
            const int selectedIndex = 1;
            
            Rating capturedRating = null;
            _mockRatingViewModel.SetupSet(vm => vm.SelectedRating = It.IsAny<Rating>())
                .Callback<Rating>(r => capturedRating = r);

            // Act
            _viewModel.HandleRatingSelectionInternal(selectedIndex);

            // Assert - verify the correct rating was set
            Assert.That(capturedRating, Is.Not.Null);
            Assert.That(capturedRating.RatingId, Is.EqualTo(_ratings[selectedIndex].RatingId));
        }

        [Test]
        public void HandleRatingSelection_WithValidIndex_LoadsReviewsForRating()
        {
            // Arrange
            const int selectedIndex = 2;
            
            int capturedRatingId = 0;
            _mockReviewViewModel.Setup(vm => vm.LoadReviewsForRating(It.IsAny<int>()))
                .Callback<int>(id => capturedRatingId = id);

            // Act
            _viewModel.HandleRatingSelectionInternal(selectedIndex);

            // Assert - verify the correct rating ID was passed
            Assert.That(capturedRatingId, Is.EqualTo(_ratings[selectedIndex].RatingId));
        }

        [Test]
        public void HandleRatingSelection_WithNegativeIndex_DoesNotSetSelectedRating()
        {
            // Arrange
            const int invalidIndex = -1;
            
            bool selectedRatingWasSet = false;
            _mockRatingViewModel.SetupSet(vm => vm.SelectedRating = It.IsAny<Rating>())
                .Callback<Rating>(_ => selectedRatingWasSet = true);
            
            bool loadReviewsWasCalled = false;
            _mockReviewViewModel.Setup(vm => vm.LoadReviewsForRating(It.IsAny<int>()))
                .Callback<int>(_ => loadReviewsWasCalled = true);

            // Act
            _viewModel.HandleRatingSelectionInternal(invalidIndex);

            // Assert
            Assert.That(selectedRatingWasSet, Is.False, "SelectedRating should not have been set");
            Assert.That(loadReviewsWasCalled, Is.False, "LoadReviewsForRating should not have been called");
        }

        [Test]
        public void HandleRatingSelection_WithOutOfRangeIndex_DoesNotThrowException()
        {
            // Arrange
            const int outOfRangeIndex = 10; // Beyond the collection size

            // Act & Assert
            Assert.DoesNotThrow(() => _viewModel.HandleRatingSelectionInternal(outOfRangeIndex));
        }
    }
} 