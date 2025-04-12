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

    [Test]
    public void TestDatabaseRatingRepository_FindByProductId_NoProduct()
    {
        var productRatings = _repository.FindByProductId(0);

        Assert.That(productRatings, Is.Not.Null);
        Assert.That(productRatings, Is.Empty);
    }


    [Test]
    public void TestDatabaseRatingRepository_FindByProductId()
    {
        var productRatings = _repository.FindByProductId(101);

        Assert.That(productRatings, Is.Not.Null);
        Assert.That(productRatings, Is.Not.Empty);

        Assert.That(productRatings.Count, Is.EqualTo(1));
    }
}
