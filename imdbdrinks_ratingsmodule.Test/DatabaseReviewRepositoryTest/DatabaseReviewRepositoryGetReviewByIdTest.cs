using imdbdrinks_ratingsmodule.Constants.ErrorMessages;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseReviewRepositoryGetReviewByIdTest
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
    public void GetReviewById_ExistingId_ReturnsCorrectRating()
    {
        var review = _repository.GetReviewById(ExistentReviewId);

        Assert.That(review.ReviewId, Is.EqualTo(ExistentReviewId));
    }

    private const int NonExistentReviewId = 100;
    [Test]
    public void GetReviewById_NonExistingId_ThrowsInvalidOperationException()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.GetReviewById(NonExistentReviewId));
        Assert.That(exception.Message, Is.EqualTo(ReviewRepositoryErrorMessages.ExhaustSingleReviewReaderInvalidReader));
    }
}
