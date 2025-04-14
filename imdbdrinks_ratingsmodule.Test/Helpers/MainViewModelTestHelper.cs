using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Xaml.Controls;
using Moq;
using System.Collections.ObjectModel;

namespace imdbdrinks_ratingsmodule.Test.Helpers
{
    public static class MainViewModelTestHelper
    {
        public static Mock<IConfiguration> CreateMockConfiguration()
        {
            return new Mock<IConfiguration>();
        }
        
        public static Mock<RatingViewModel> CreateMockRatingViewModel()
        {
            var mockViewModel = new Mock<RatingViewModel>(new Mock<RatingService>(new Mock<IRatingRepository>().Object).Object);
            mockViewModel.SetupAllProperties();
            mockViewModel.Object.Ratings = new ObservableCollection<Rating>();
            return mockViewModel;
        }
        
        public static Mock<ReviewViewModel> CreateMockReviewViewModel()
        {
            var mockViewModel = new Mock<ReviewViewModel>(new Mock<ReviewService>(new Mock<IReviewRepository>().Object).Object);
            mockViewModel.SetupAllProperties();
            mockViewModel.Object.Reviews = new ObservableCollection<Review>();
            return mockViewModel;
        }
        
        public static MainViewModel CreateMainViewModel(
            Mock<IConfiguration> mockConfiguration = null,
            Mock<RatingViewModel> mockRatingViewModel = null,
            Mock<ReviewViewModel> mockReviewViewModel = null)
        {
            mockConfiguration ??= CreateMockConfiguration();
            mockRatingViewModel ??= CreateMockRatingViewModel();
            mockReviewViewModel ??= CreateMockReviewViewModel();
            
            return new MainViewModel(
                mockConfiguration.Object,
                mockRatingViewModel.Object,
                mockReviewViewModel.Object);
        }
    }
} 