using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Test.Helpers;
using imdbdrinks_ratingsmodule.ViewModels;
using Moq;

namespace imdbdrinks_ratingsmodule.Test
{
    public class MainViewModelClearSelectedRatingTest
    {
        private MainViewModel _viewModel;
        private Mock<RatingViewModel> _mockRatingViewModel;
        private Mock<ReviewViewModel> _mockReviewViewModel;

        [SetUp]
        public void Setup()
        {
            _mockRatingViewModel = MainViewModelTestHelper.CreateMockRatingViewModel();
            _mockReviewViewModel = MainViewModelTestHelper.CreateMockReviewViewModel();
            
            _viewModel = MainViewModelTestHelper.CreateMainViewModel(
                mockRatingViewModel: _mockRatingViewModel, 
                mockReviewViewModel: _mockReviewViewModel);
        }

        [Test]
        public void ClearSelectedRating_SetsSelectedRatingToNull()
        {
            // Arrange
            var rating = new Rating { RatingId = 1, ProductId = 100, RatingValue = 4 };
            _mockRatingViewModel.Object.SelectedRating = rating;

            bool wasSetToNull = false;
            _mockRatingViewModel.SetupSet(vm => vm.SelectedRating = null)
                .Callback(() => wasSetToNull = true);

            // Act
            _viewModel.ClearSelectedRating();

            // Assert
            Assert.That(wasSetToNull, Is.True, "SelectedRating was not set to null");
        }

        [Test]
        public void ClearSelectedRating_WithNullSelectedRating_StillClearsRating()
        {
            // Arrange - selected rating is already null
            _mockRatingViewModel.Object.SelectedRating = null;

            bool wasSetToNull = false;
            _mockRatingViewModel.SetupSet(vm => vm.SelectedRating = null)
                .Callback(() => wasSetToNull = true);

            // Act
            _viewModel.ClearSelectedRating();

            // Assert - method should still be called
            Assert.That(wasSetToNull, Is.True, "SelectedRating was not set to null");
        }

        [Test]
        public void ClearSelectedRating_DoesNotAffectOtherProperties()
        {
            // Arrange
            var rating = new Rating { RatingId = 1, ProductId = 100, RatingValue = 4 };
            _mockRatingViewModel.Object.SelectedRating = rating;
            
            // Set up some other properties that shouldn't change
            var ratings = new System.Collections.ObjectModel.ObservableCollection<Rating> { rating };
            _mockRatingViewModel.Object.Ratings = ratings;
            
            // Store the initial state to compare after the method call
            var initialRatings = _mockRatingViewModel.Object.Ratings;
            
            // Track if SelectedRating is set to null
            bool selectedRatingWasSetToNull = false;
            _mockRatingViewModel.SetupSet(vm => vm.SelectedRating = null)
                .Callback(() => selectedRatingWasSetToNull = true);
                
            // Act
            _viewModel.ClearSelectedRating();
            
            // Assert
            Assert.That(selectedRatingWasSetToNull, Is.True, "SelectedRating was not set to null");
            
            // Verify Ratings collection is unchanged by comparing references
            Assert.That(_mockRatingViewModel.Object.Ratings, Is.SameAs(initialRatings));
            
            // No need to verify ReviewViewModel as it's not touched by this method
        }
    }
} 