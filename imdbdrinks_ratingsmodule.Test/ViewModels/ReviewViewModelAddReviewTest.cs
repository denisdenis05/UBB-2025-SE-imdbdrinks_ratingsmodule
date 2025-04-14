using System;
using System.Collections.Generic;
using System.Linq;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Services;
using imdbdrinks_ratingsmodule.ViewModels;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Constants.ErrorMessages;
using Moq;
using NUnit.Framework;

namespace imdbdrinks_ratingsmodule.Test;

public class ReviewViewModelAddReviewTest
{
    private Mock<IReviewRepository> _reviewRepositoryMock;
    private ReviewService _reviewService;
    private ReviewViewModel _viewModel;
    
    private const int TestRatingId = 1;
    private const int DefaultUserId = 999;
    private const string TestReviewContent = "Great product!";
    private const string EmptyReviewContent = "   ";
    private const int ExpectedReviewCount = 1;
    private const int EmptyReviewCount = 0;
    private const int MaxContentLength = 500;

    [SetUp]
    public void Setup()
    {
        _reviewRepositoryMock = new Mock<IReviewRepository>();
        _reviewService = new ReviewService(_reviewRepositoryMock.Object);
        _viewModel = new ReviewViewModel(_reviewService);
    }

    [Test]
    public void AddReview_WhenValidContent_AddsReviewAndClearsContent()
    {
        // Arrange
        _viewModel.ReviewContent = TestReviewContent;
        
        _reviewRepositoryMock.Setup(repository => repository.AddOrUpdateReview(It.IsAny<Review>()))
            .Returns<Review>(review => review);

        var expectedReviews = new List<Review>
        {
            new Review 
            { 
                RatingId = TestRatingId, 
                UserId = DefaultUserId, 
                Content = TestReviewContent, 
                IsActive = true 
            }
        };
        _reviewRepositoryMock.Setup(repository => repository.GetReviewsByRatingId(TestRatingId))
            .Returns(expectedReviews);

        // Act
        _viewModel.AddReview(TestRatingId);

        // Assert
        Assert.That(_viewModel.ReviewContent, Is.Empty, "Review content should be cleared after adding review");
        Assert.That(_viewModel.Reviews.Count, Is.EqualTo(ExpectedReviewCount), "Should have exactly one review");
        Assert.That(_viewModel.Reviews[0].Content, Is.EqualTo(TestReviewContent), "Review content should match input");
        Assert.That(_viewModel.Reviews[0].RatingId, Is.EqualTo(TestRatingId), "Review should be associated with correct rating");
        Assert.That(_viewModel.Reviews[0].UserId, Is.EqualTo(DefaultUserId), "Review should have correct user ID");
        Assert.That(_viewModel.Reviews[0].IsActive, Is.True, "Review should be active");
    }

    [Test]
    public void AddReview_WhenEmptyContent_DoesNotAddReview()
    {
        // Arrange
        _viewModel.ReviewContent = EmptyReviewContent;

        // Act
        _viewModel.AddReview(TestRatingId);

        // Assert
        _reviewRepositoryMock.Verify(repository => repository.AddOrUpdateReview(It.IsAny<Review>()), Times.Never, 
            "Repository should not be called when content is empty");
        Assert.That(_viewModel.Reviews.Count, Is.EqualTo(EmptyReviewCount), "No reviews should be added when content is empty");
    }

    [Test]
    public void AddReview_WhenContentTooLong_ThrowsArgumentException()
    {
        // Arrange
        var longContent = new string('a', MaxContentLength + 1);
        _viewModel.ReviewContent = longContent;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _viewModel.AddReview(TestRatingId));
        Assert.That(exception.Message, Is.EqualTo(ReviewServiceErrorMessages.InvalidReview), 
            "Should throw ArgumentException with correct error message");
        _reviewRepositoryMock.Verify(repository => repository.AddOrUpdateReview(It.IsAny<Review>()), Times.Never,
            "Repository should not be called when content exceeds maximum length");
    }

    [Test]
    public void AddReview_WhenMultipleReviews_AddsAllReviewsCorrectly()
    {
        // Arrange
        var firstReviewContent = "First review";
        var secondReviewContent = "Second review";
        
        _reviewRepositoryMock.Setup(repository => repository.AddOrUpdateReview(It.IsAny<Review>()))
            .Returns<Review>(review => review);

        var expectedReviews = new List<Review>
        {
            new Review { RatingId = TestRatingId, UserId = DefaultUserId, Content = firstReviewContent, IsActive = true },
            new Review { RatingId = TestRatingId, UserId = DefaultUserId, Content = secondReviewContent, IsActive = true }
        };
        _reviewRepositoryMock.Setup(repository => repository.GetReviewsByRatingId(TestRatingId))
            .Returns(expectedReviews);

        // Act
        _viewModel.ReviewContent = firstReviewContent;
        _viewModel.AddReview(TestRatingId);
        _viewModel.ReviewContent = secondReviewContent;
        _viewModel.AddReview(TestRatingId);

        // Assert
        Assert.That(_viewModel.Reviews.Count, Is.EqualTo(2), "Should have two reviews");
        Assert.That(_viewModel.Reviews[0].Content, Is.EqualTo(firstReviewContent), "First review content should match");
        Assert.That(_viewModel.Reviews[1].Content, Is.EqualTo(secondReviewContent), "Second review content should match");
    }

    [Test]
    public void AddReview_WhenCalled_SetsCreationDate()
    {
        // Arrange
        _viewModel.ReviewContent = TestReviewContent;
        var testStartTime = DateTime.Now;
        
        _reviewRepositoryMock.Setup(repository => repository.AddOrUpdateReview(It.IsAny<Review>()))
            .Returns<Review>(review => review);

        _reviewRepositoryMock.Setup(repository => repository.GetReviewsByRatingId(TestRatingId))
            .Returns(new List<Review> { new Review() });

        // Act
        _viewModel.AddReview(TestRatingId);

        // Assert
        _reviewRepositoryMock.Verify(repository => repository.AddOrUpdateReview(
            It.Is<Review>(r => r.CreationDate >= testStartTime)), 
            Times.Once, "Review should have a creation date set to current time");
    }

    [Test]
    public void AddReview_WhenCalled_RaisesPropertyChangedForReviewContent()
    {
        // Arrange
        var propertyChangedRaised = false;
        _viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(ReviewViewModel.ReviewContent))
            {
                propertyChangedRaised = true;
            }
        };

        _viewModel.ReviewContent = TestReviewContent;
        _reviewRepositoryMock.Setup(repository => repository.AddOrUpdateReview(It.IsAny<Review>()))
            .Returns<Review>(review => review);
        _reviewRepositoryMock.Setup(repository => repository.GetReviewsByRatingId(TestRatingId))
            .Returns(new List<Review> { new Review() });

        // Act
        _viewModel.AddReview(TestRatingId);

        // Assert
        Assert.That(propertyChangedRaised, Is.True, "PropertyChanged event should be raised for ReviewContent");
    }
} 