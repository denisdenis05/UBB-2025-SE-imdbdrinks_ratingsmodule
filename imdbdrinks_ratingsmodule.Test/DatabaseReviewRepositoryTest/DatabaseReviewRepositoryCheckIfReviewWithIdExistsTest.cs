using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseReviewRepositoryCheckIfReviewWithIdExistsTest
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

    private const int ExistentReviewId = 1;
    [Test]
    public void CheckIfReviewWithIdExists_ExistentReviewId_ReturnsTrue()
    {
        bool existsReview = _repository.CheckIfReviewWithIdExists(ExistentReviewId);
        Assert.That(existsReview, Is.EqualTo(true));
    }

    private const int NonExistentReviewId = 999;
    [Test]
    public void CheckIfReviewWithIdExists_NonExistentReviewId_ReturnsFalse()
    {
        bool existsReview = _repository.CheckIfReviewWithIdExists(NonExistentReviewId);
        Assert.That(existsReview, Is.EqualTo(false));
    }
}
