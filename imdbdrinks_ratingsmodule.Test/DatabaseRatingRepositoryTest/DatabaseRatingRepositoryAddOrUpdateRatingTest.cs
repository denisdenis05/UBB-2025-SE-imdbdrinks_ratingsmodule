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


    [Test]
    public void AddOrUpdateRating_NewRating_AddsRatingToRepository()
    {
        var allRatings = _repository.GetAllRatings();
        Assert.That(allRatings, Is.Not.Null);
        Assert.That(allRatings.Count, Is.EqualTo(3));
        var newRating = new Rating
        {
            UserId = 1,
            ProductId = 101,
            RatingValue = 5,
            RatingDate = DateTime.Now,
            IsActive = true
        };
        _repository.AddOrUpdateRating(newRating);
        allRatings = _repository.GetAllRatings();
        Assert.That(allRatings, Is.Not.Null);
        Assert.That(allRatings.Count, Is.EqualTo(4));
        var savedRating = allRatings.FirstOrDefault(r => r.ProductId == 101 && r.RatingValue == 5);
        Assert.That(savedRating, Is.Not.Null);
    }


    private const int ExistentRatingId = 1;
    [Test]
    public void AddOrUpdateRating_ExistingRating_UpdatesRatingInRepository()
    {
        var allRatings = _repository.GetAllRatings();
        Assert.That(allRatings, Is.Not.Null);
        Assert.That(allRatings.Count, Is.EqualTo(3));
        var newRating = new Rating
        {
            RatingId = ExistentRatingId, // Assuming this ID exists in the test database
            UserId = 1,
            ProductId = 101,
            RatingValue = 5,
            RatingDate = DateTime.Now,
            IsActive = true
        };
        _repository.AddOrUpdateRating(newRating);
        allRatings = _repository.GetAllRatings();
        Assert.That(allRatings, Is.Not.Null);
        Assert.That(allRatings.Count, Is.EqualTo(3));
        var savedRating = allRatings.FirstOrDefault(r => r.ProductId == 101 && r.RatingValue == 5);
        Assert.That(savedRating, Is.Not.Null);
    }

}
