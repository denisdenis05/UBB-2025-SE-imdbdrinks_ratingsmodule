using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;
using Moq;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseRatingRepositorySaveTest
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
    public void TestDatabaseRatingRepository_Save()
    {
        var allRatings = _repository.FindAll();
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
        _repository.Save(newRating);
        allRatings = _repository.FindAll();
        Assert.That(allRatings, Is.Not.Null);
        Assert.That(allRatings.Count, Is.EqualTo(4));
        var savedRating = allRatings.FirstOrDefault(r => r.ProductId == 101 && r.RatingValue == 5);
        Assert.That(savedRating, Is.Not.Null);
    }


    private const int EXISTENT_RATING_ID = 1;
    [Test]
    public void TestDatabaseRatingRepository_UpdateOnSave()
    {
        var allRatings = _repository.FindAll();
        Assert.That(allRatings, Is.Not.Null);
        Assert.That(allRatings.Count, Is.EqualTo(3));
        var newRating = new Rating
        {
            RatingId = EXISTENT_RATING_ID, // Assuming this ID exists in the test database
            UserId = 1,
            ProductId = 101,
            RatingValue = 5,
            RatingDate = DateTime.Now,
            IsActive = true
        };
        _repository.Save(newRating);
        allRatings = _repository.FindAll();
        Assert.That(allRatings, Is.Not.Null);
        Assert.That(allRatings.Count, Is.EqualTo(3));
        var savedRating = allRatings.FirstOrDefault(r => r.ProductId == 101 && r.RatingValue == 5);
        Assert.That(savedRating, Is.Not.Null);
    }

}
