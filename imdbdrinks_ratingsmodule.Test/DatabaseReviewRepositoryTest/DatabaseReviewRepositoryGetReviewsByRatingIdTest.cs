using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseReviewRepositoryGetReviewsByRatingIdTest
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

    private const int NonExistentRatingId = 999;
    [Test]
    public void GetReviewsByRatingId_NonExistingRatingId_ReturnsEmptyCollection()
    {
        var productReviews = _repository.GetReviewsByRatingId(NonExistentRatingId);

        Assert.That(productReviews, Is.Empty);
    }

    private const int ExistentRatingId = 1;
    private const int ExpectedNumberOfRatings = 2;
    [Test]
    public void GetReviewsByRatingId_ExistingRatingId_ReturnsCorrectReviews()
    {
        var productRatings = _repository.GetReviewsByRatingId(ExistentRatingId);

        Assert.That(productRatings.Count, Is.EqualTo(ExpectedNumberOfRatings));
    }
}
