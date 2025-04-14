using System;
using System.Collections.Generic;
using System.Linq;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.ViewModels;
using imdbdrinks_ratingsmodule.Repositories;
using Moq;
using NUnit.Framework;

namespace imdbdrinks_ratingsmodule.Test;

public class ReviewViewModelLoadReviewsTest
{
    private Mock<IReviewRepository> _reviewRepositoryMock;
    private ReviewService _reviewService;
    private ReviewViewModel _viewModel;
    
    private const int TestRatingId = 1;
    private const int NonExistentRatingId = 999;
    private const int DefaultUserId = 999;
    private const string FirstReviewContent = "First review";
    private const string SecondReviewContent = "Second review";
    private const int ExpectedReviewsCount = 2;
    private const int EmptyReviewsCount = 0;

    [SetUp]
    public void Setup()
    {
        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _reviewService = new ReviewService(_reviewRepositoryMock.Object);
        _viewModel = new ReviewViewModel(_reviewService);
    }

    [Test]
    public void LoadReviewsForRating_WhenReviewsExist_LoadsAllReviews()
    {
        // Arrange
        var expectedReviews = new List<Review>
        {
            new Review 
            { 
                RatingId = TestRatingId, 
                UserId = DefaultUserId, 
                Content = FirstReviewContent, 
                IsActive = true 
            },
            new Review 
            { 
                RatingId = TestRatingId, 
                UserId = DefaultUserId, 
                Content = SecondReviewContent, 
                IsActive = true 
            }
        };
        _reviewRepositoryMock.Setup(repository => repository.GetReviewsByRatingId(TestRatingId))
            .Returns(expectedReviews);

        // Act
        _viewModel.LoadReviewsForRating(TestRatingId);

        // Assert
        Assert.That(_viewModel.Reviews.Count, Is.EqualTo(ExpectedReviewsCount), "Should load all reviews");
        Assert.That(_viewModel.Reviews[0].Content, Is.EqualTo(FirstReviewContent), "First review content should match");
        Assert.That(_viewModel.Reviews[1].Content, Is.EqualTo(SecondReviewContent), "Second review content should match");
        Assert.That(_viewModel.Reviews[0].RatingId, Is.EqualTo(TestRatingId), "First review should have correct rating ID");
        Assert.That(_viewModel.Reviews[1].RatingId, Is.EqualTo(TestRatingId), "Second review should have correct rating ID");
    }

    [Test]
    public void LoadReviewsForRating_WhenNoReviewsExist_ReturnsEmptyCollection()
    {
        // Arrange
        _reviewRepositoryMock.Setup(repository => repository.GetReviewsByRatingId(TestRatingId))
            .Returns(new List<Review>());

        // Act
        _viewModel.LoadReviewsForRating(TestRatingId);

        // Assert
        Assert.That(_viewModel.Reviews.Count, Is.EqualTo(EmptyReviewsCount), "Should return empty collection when no reviews exist");
    }

    [Test]
    public void LoadReviewsForRating_WhenCalledWithNonExistentRating_ReturnsEmptyCollection()
    {
        // Arrange
        _reviewRepositoryMock.Setup(repository => repository.GetReviewsByRatingId(NonExistentRatingId))
            .Returns(new List<Review>());

        // Act
        _viewModel.LoadReviewsForRating(NonExistentRatingId);

        // Assert
        Assert.That(_viewModel.Reviews.Count, Is.EqualTo(EmptyReviewsCount), "Should return empty collection for non-existent rating");
    }

    [Test]
    public void LoadReviewsForRating_WhenCalled_ReplacesExistingReviews()
    {
        // Arrange
        var initialReviews = new List<Review>
        {
            new Review { RatingId = TestRatingId, Content = "Old review" }
        };
        _viewModel.LoadReviewsForRating(TestRatingId);

        var newReviews = new List<Review>
        {
            new Review { RatingId = TestRatingId, Content = FirstReviewContent },
            new Review { RatingId = TestRatingId, Content = SecondReviewContent }
        };
        _reviewRepositoryMock.Setup(repository => repository.GetReviewsByRatingId(TestRatingId))
            .Returns(newReviews);

        // Act
        _viewModel.LoadReviewsForRating(TestRatingId);

        // Assert
        Assert.That(_viewModel.Reviews.Count, Is.EqualTo(ExpectedReviewsCount), "Should replace old reviews with new ones");
        Assert.That(_viewModel.Reviews[0].Content, Is.EqualTo(FirstReviewContent), "First new review should be present");
        Assert.That(_viewModel.Reviews[1].Content, Is.EqualTo(SecondReviewContent), "Second new review should be present");
    }
} 