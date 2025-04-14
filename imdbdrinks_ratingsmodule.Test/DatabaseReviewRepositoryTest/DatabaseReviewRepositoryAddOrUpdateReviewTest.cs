using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseReviewRepositoryAddOrUpdateReviewTest
{
    private IReviewRepository _repository;
    private TestDatabaseHelper _testDatabaseHelper;

    [SetUp]
    public void Setup()
    {
        IConfiguration configuration = TestDatabaseHelper.CreateTestConfiguration();
        _testDatabaseHelper = new TestDatabaseHelper(configuration);
        DatabaseConnection databaseConnection = new DatabaseConnection(configuration);

        _testDatabaseHelper.PrepareTestDatabase();

        _repository = new DatabaseReviewRepository(databaseConnection);
    }

    private const int InitialNumberOfReviews = 3;
    private const int NewNumberOfReviews = InitialNumberOfReviews + 1;
    private const int SampleUserId = 1;
    private const int SampleRatingId = 1;
    private const string SampleContent = "Sample text";
    [Test]
    public void AddOrUpdateReview_NewReview_AddsReviewToRepository()
    {
        var newReview = new Review
        {
            UserId = SampleUserId,
            RatingId = SampleRatingId,
            CreationDate = DateTime.Now,
            Content = SampleContent,
            IsActive = true
        };

        _repository.AddOrUpdateReview(newReview);

        var allRatings = _repository.GetAllReviews();

        Assert.That(allRatings.Count, Is.EqualTo(NewNumberOfReviews));
    }

    private const int ExistentReviewId = 1;
    [Test]
    public void AddOrUpdateReview_ExistingReview_UpdatesReviewInRepository()
    {
        var existingReview = new Review
        {
            ReviewId = ExistentReviewId,
            UserId = SampleUserId,
            RatingId = SampleRatingId,
            CreationDate = DateTime.Now,
            Content = SampleContent,
            IsActive = true
        };

        _repository.AddOrUpdateReview(existingReview);
        var allReviews = _repository.GetAllReviews();

        Assert.That(allReviews.Count, Is.EqualTo(InitialNumberOfReviews));
    }
}
