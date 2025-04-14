using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;
using Moq;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseRatingRepositoryGetRatingByIdTest
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


    private const int ExistentRatingId = 1;
    [Test]
    public void GetRatingById_ExistingId_ReturnsCorrectRating()
    {
        var rating = _repository.GetRatingById(ExistentRatingId);

        Assert.That(rating.RatingId, Is.EqualTo(ExistentRatingId));
    }

    private const int NonExistentRatingId = 100;
    [Test]
    public void GetRatingById_NonExistingId_ReturnsNull()
    {
        var rating = _repository.GetRatingById(NonExistentRatingId);

        Assert.That(rating, Is.Null);
    }

}
