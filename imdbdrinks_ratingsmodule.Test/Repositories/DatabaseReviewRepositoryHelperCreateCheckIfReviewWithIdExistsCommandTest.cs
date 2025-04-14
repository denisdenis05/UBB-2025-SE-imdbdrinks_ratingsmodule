using System;
using Microsoft.Data.SqlClient;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Queries;
using NUnit.Framework;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseReviewRepositoryHelperCreateCheckIfReviewWithIdExistsCommandTest
{
    private SqlConnection _connection;
    private const int TestReviewId = 1;

    [SetUp]
    public void Setup()
    {
        _connection = new SqlConnection("Server=.;Database=TestDB;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    [TearDown]
    public void TearDown()
    {
        _connection?.Dispose();
    }

    [Test]
    public void CreateCheckIfReviewWithIdExistsCommand_WhenCalled_CreatesCommandWithCorrectParameters()
    {
        // Act
        var command = DatabaseReviewRepositoryHelper.CreateCheckIfReviewWithIdExistsCommand(_connection, TestReviewId);

        // Assert
        Assert.That(command.CommandText, Is.EqualTo(ReviewQueries.CheckIfIdExistsQuery), "Command text should match the query");
        Assert.That(command.Connection, Is.EqualTo(_connection), "Connection should be set correctly");
        Assert.That(command.Parameters["@ReviewId"].Value, Is.EqualTo(TestReviewId), "ReviewId parameter should be set correctly");
    }

    [Test]
    public void CreateCheckIfReviewWithIdExistsCommand_WhenCalledWithNullConnection_CreatesCommandButCannotExecute()
    {
        // Act
        var command = DatabaseReviewRepositoryHelper.CreateCheckIfReviewWithIdExistsCommand(null!, TestReviewId);

        // Assert
        Assert.That(command, Is.Not.Null, "Command should be created");
        Assert.That(command.Connection, Is.Null, "Command's connection should be null");
        Assert.Throws<InvalidOperationException>(() => command.ExecuteScalar(), 
            "Should throw InvalidOperationException when trying to execute command with null connection");
    }

    [Test]
    public void CreateCheckIfReviewWithIdExistsCommand_WhenCalledWithNegativeReviewId_CreatesCommandWithNegativeId()
    {
        // Act
        var command = DatabaseReviewRepositoryHelper.CreateCheckIfReviewWithIdExistsCommand(_connection, -1);

        // Assert
        Assert.That(command, Is.Not.Null, "Command should be created");
        Assert.That(command.Parameters["@ReviewId"].Value, Is.EqualTo(-1), "ReviewId parameter should be set to negative value");
    }
} 