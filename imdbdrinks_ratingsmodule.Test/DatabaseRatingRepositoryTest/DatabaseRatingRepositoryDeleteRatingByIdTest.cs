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

    private const int RatingIdToBeDeleted = 3;
    private const int InitialNumberOfRatings = 3;
    private const int AfterDeleteNumberOfRatings = InitialNumberOfRatings - 1;
    [Test]
    public void DeleteRatingById_ExistingRating_RemovesRatingFromRepository()
    {

        _repository.DeleteRatingById(RatingIdToBeDeleted);

        var allRatings = _repository.GetAllRatings();

        Assert.That(allRatings.Count, Is.EqualTo(AfterDeleteNumberOfRatings));
    }

}
