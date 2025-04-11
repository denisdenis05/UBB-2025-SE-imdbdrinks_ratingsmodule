using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Test.Helpers;
using Microsoft.Extensions.Configuration;
using Moq;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseRatingRepositoryFindAllTest
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
    public void TestDatabaseRatingRepository_FindAll()
    {
        var allRatings = _repository.FindAll();

        Assert.That(allRatings, Is.Not.Null);

        Assert.That(allRatings, Is.Not.Empty);

        Assert.That(allRatings.Count,Is.EqualTo(3));

    }
}
