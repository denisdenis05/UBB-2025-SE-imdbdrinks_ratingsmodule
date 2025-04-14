using System.Collections.Generic;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.ViewModels;
using Moq;

namespace imdbdrinks_ratingsmodule.Test.Helpers
{
    // Interface for mocking the RatingService
    public interface IRatingService
    {
        Rating GetRatingById(int ratingId);
        IEnumerable<Rating> GetRatingsByProduct(int productId);
        Rating CreateRating(Rating rating);
        Rating UpdateRating(Rating rating);
        void DeleteRatingById(int ratingId);
        double GetAverageRating(int productId);
    }

    public static class RatingViewModelTestHelper
    {
        public static Mock<IRatingService> CreateMockRatingService()
        {
            return new Mock<IRatingService>();
        }
        
        public static RatingViewModel CreateRatingViewModel(Mock<IRatingService> mockRatingService = null)
        {
            mockRatingService ??= CreateMockRatingService();
            // We need to adapt our mocked interface to work with the RatingViewModel that expects a RatingService
            var mockRatingServiceWrapper = new RatingServiceTestWrapper(mockRatingService.Object);
            return new RatingViewModel(mockRatingServiceWrapper);
        }
        
        public static List<Rating> CreateSampleRatings(int productId, int count = 3)
        {
            var ratings = new List<Rating>();
            
            for (int i = 1; i <= count; i++)
            {
                ratings.Add(new Rating
                {
                    RatingId = i,
                    ProductId = productId,
                    UserId = i,
                    RatingValue = i + 1, // Values from 2 to count+1
                    IsActive = true,
                    RatingDate = System.DateTime.Now.AddDays(-i) // Older ratings have earlier dates
                });
            }
            
            return ratings;
        }

        public static void SetupGetRatingsByProduct(
            Mock<IRatingService> mockService, 
            int productId, 
            List<Rating> ratings)
        {
            mockService.Setup(service => service.GetRatingsByProduct(productId))
                .Returns(ratings);
        }
        
        public static void SetupGetAverageRating(
            Mock<IRatingService> mockService,
            int productId,
            double averageValue)
        {
            mockService.Setup(service => service.GetAverageRating(productId))
                .Returns(averageValue);
        }
    }
    
    // Wrapper class that implements RatingService's functionality using our mockable interface
    public class RatingServiceTestWrapper : RatingService
    {
        private readonly IRatingService _mockRatingService;
        
        public RatingServiceTestWrapper(IRatingService mockRatingService) 
            : base(new Mock<IRatingRepository>().Object) // Pass a dummy repository to the base constructor
        {
            _mockRatingService = mockRatingService;
        }
        
        public override Rating GetRatingById(int ratingId) => _mockRatingService.GetRatingById(ratingId);
        
        public override IEnumerable<Rating> GetRatingsByProduct(int productId) => _mockRatingService.GetRatingsByProduct(productId);
        
        public override Rating CreateRating(Rating rating) => _mockRatingService.CreateRating(rating);
        
        public override Rating UpdateRating(Rating rating) => _mockRatingService.UpdateRating(rating);
        
        public override void DeleteRatingById(int ratingId) => _mockRatingService.DeleteRatingById(ratingId);
        
        public override double GetAverageRating(int productId) => _mockRatingService.GetAverageRating(productId);
    }
} 