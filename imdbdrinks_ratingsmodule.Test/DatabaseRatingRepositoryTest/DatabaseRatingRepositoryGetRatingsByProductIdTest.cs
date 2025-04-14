using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseRatingRepositoryGetRatingsByProductIdTest
{

    private IRatingRepository _repository;
    private TestDatabaseHelper _testDatabaseHelper;

    [SetUp]
    public void Setup()
    {
        IConfiguration configuration = TestDatabaseHelper.CreateTestConfiguration();
        _testDatabaseHelper = new TestDatabaseHelper(configuration);
        DatabaseConnection databaseConnection = new DatabaseConnection(configuration);

        _testDatabaseHelper.PrepareTestDatabase();

        _repository = new DatabaseRatingRepository(databaseConnection);
    }

    private const int NonExistentProductId = 0;
    [Test]
    public void GetRatingsByProductId_NonExistingProductId_ReturnsEmptyCollection()
    {
        var productRatings = _repository.GetRatingsByProductId(NonExistentProductId);

        Assert.That(productRatings, Is.Empty);
    }

    private const int ExistentProductId = 101;
    private const int ExpectedNumberOfRatings = 1;
    [Test]
    public void GetRatingsByProductId_ExistingProductId_ReturnsCorrectRating()
    {
        var productRatings = _repository.GetRatingsByProductId(ExistentProductId);

        Assert.That(productRatings.Count, Is.EqualTo(ExpectedNumberOfRatings));
    }
}
