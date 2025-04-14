using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseRatingRepositoryFindByProductIdTest
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

    private const int NON_EXISTENT_PRODUCT_ID = 0;
    [Test]
    public void FindByProductId_NonExistingProductId_ReturnsEmptyCollection()
    {
        var productRatings = _repository.FindByProductId(NON_EXISTENT_PRODUCT_ID);

        Assert.That(productRatings, Is.Not.Null);
        Assert.That(productRatings, Is.Empty);
    }

    private const int EXISTENT_PRODUCT_ID = 101;
    [Test]
    public void TestDatabaseRatingRepository_FindByProductId()
    {
        var productRatings = _repository.FindByProductId(EXISTENT_PRODUCT_ID);

        Assert.That(productRatings, Is.Not.Null);
        Assert.That(productRatings.Count, Is.EqualTo(1));
    }
}
