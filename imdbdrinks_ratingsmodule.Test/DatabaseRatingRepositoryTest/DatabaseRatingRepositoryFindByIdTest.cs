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

    [Test]
    public void TestDatabaseRatingRepository_FindById()
    {
        // "VALUE (101, 1, 4, '2025-04-01 10:30:00', 1)
        var rating = _repository.FindById(1);
        Assert.That(rating, Is.Not.Null);
        Assert.That(rating.ProductId, Is.EqualTo(101));
        Assert.That(rating.RatingValue, Is.EqualTo(4));
        Assert.That(rating.RatingDate, Is.EqualTo(new DateTime(2025, 4, 1, 10, 30, 0)));
        Assert.That(rating.IsActive, Is.EqualTo(true));
    }

    public void TestDatabaseRatingRepository_FindById_NotFound()
    {
        var rating = _repository.FindById(100);
        Assert.That(rating, Is.Null);
    }
}
