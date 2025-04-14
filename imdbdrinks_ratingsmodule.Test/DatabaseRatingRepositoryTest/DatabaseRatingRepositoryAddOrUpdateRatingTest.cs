using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;
using Moq;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseRatingRepositoryAddOrUpdateRatingTest
{
    private IRatingRepository _repository;
    private TestDatabaseHelper _testDatabaseHelper;

    [SetUp]
    public void Setup()
    {
        IConfiguration configuration = TestDatabaseHelper.CreateTestConfiguration();
        _testDatabaseHelper = new TestDatabaseHelper(configuration);
        Mock<DatabaseConnection> databaseConnection = new Mock<DatabaseConnection>(configuration);

        _testDatabaseHelper.PrepareTestDatabase();

        _repository = new DatabaseRatingRepository(databaseConnection.Object);
    }

    private const int InitialNumberOfRatings = 3;
    private const int NewNumberOfRatings = InitialNumberOfRatings + 1;
    private const int SampleUserId = 1;
    private const int SampleProductId = 101;
    private const int NewRatingValue = 5;
    [Test]
    public void AddOrUpdateRating_NewRating_AddsRatingToRepository()
    {
        var newRating = new Rating
        {
            UserId = SampleUserId,
            ProductId = SampleProductId,
            RatingValue = NewRatingValue,
            RatingDate = DateTime.Now,
            IsActive = true
        };

        _repository.AddOrUpdateRating(newRating);

        var allRatings = _repository.GetAllRatings();

        Assert.That(allRatings.Count, Is.EqualTo(NewNumberOfRatings));

        var savedRating = allRatings.FirstOrDefault(r => r.ProductId == SampleProductId && r.RatingValue == NewRatingValue);
        Assert.That(savedRating, Is.Not.Null);
    }


    private const int ExistentRatingId = 1;
    [Test]
    public void AddOrUpdateRating_ExistingRating_UpdatesRatingInRepository()
    {
        var newRating = new Rating
        {
            RatingId = ExistentRatingId,
            UserId = SampleUserId,
            ProductId = SampleProductId,
            RatingValue = NewRatingValue,
            RatingDate = DateTime.Now,
            IsActive = true
        };

        _repository.AddOrUpdateRating(newRating);
        var allRatings = _repository.GetAllRatings();

        Assert.That(allRatings.Count, Is.EqualTo(InitialNumberOfRatings));

        var savedRating = allRatings.FirstOrDefault(r => r.ProductId == SampleProductId && r.RatingValue == NewRatingValue);
        Assert.That(savedRating, Is.Not.Null);
    }

}
