using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseReviewRepositoryDeleteReviewByIdTest
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

    private const int ReviewIdToBeDeleted = 3;
    private const int InitialNumberOfReviews = 3;
    private const int AfterDeleteNumberOfReviews = InitialNumberOfReviews - 1;
    [Test]
    public void DeleteReviewById_ExistingReview_RemovesReviewFromRepository()
    {

        _repository.DeleteReviewById(ReviewIdToBeDeleted);

        var allReviews = _repository.GetAllReviews();

        Assert.That(allReviews.Count, Is.EqualTo(AfterDeleteNumberOfReviews));
    }

}
