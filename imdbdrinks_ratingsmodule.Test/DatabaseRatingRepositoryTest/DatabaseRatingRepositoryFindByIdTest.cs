using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;
using Moq;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseRatingRepositoryFindByIdTest
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


    private const int EXISTENT_RATING_ID = 1;
    [Test]
    public void FindById_ExistingId_ReturnsCorrectRating()
    {
        // "VALUE (101, 1, 4, '2025-04-01 10:30:00', 1)
        var rating = _repository.FindById(EXISTENT_RATING_ID);
        Assert.That(rating, Is.Not.Null);
        Assert.That(rating.ProductId, Is.EqualTo(101));
        Assert.That(rating.RatingValue, Is.EqualTo(4));
        Assert.That(rating.RatingDate, Is.EqualTo(new DateTime(2025, 4, 1, 10, 30, 0)));
        Assert.That(rating.IsActive, Is.EqualTo(true));
    }

    private const int NON_EXISTENT_RATING_ID = 100;
    [Test]
    public void FindById_NonExistingId_ReturnsNull()
    {
        var rating = _repository.FindById(NON_EXISTENT_RATING_ID);
        Assert.That(rating, Is.Null);
    }

}
