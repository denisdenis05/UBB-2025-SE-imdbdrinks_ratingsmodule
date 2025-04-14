using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;
using Moq;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseRatingRepositoryDeleteTest
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
    public void Delete_ExistingRating_RemovesRatingFromRepository()
    {
        // Arrange
        var allRatings = _repository.FindAll();
        Assert.That(allRatings, Is.Not.Null);
        Assert.That(allRatings.Count, Is.EqualTo(3));

        // Act
        _repository.Delete(1);

        // Assert
        allRatings = _repository.FindAll();
        Assert.That(allRatings, Is.Not.Null);
        Assert.That(allRatings.Count, Is.EqualTo(2));
    }

}
