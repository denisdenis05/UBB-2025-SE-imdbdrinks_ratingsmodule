using System;
using Microsoft.Data.SqlClient;
using imdbdrinks_ratingsmodule.Domain;
using imdbdrinks_ratingsmodule.Repositories;
using imdbdrinks_ratingsmodule.Queries;
using NUnit.Framework;

namespace imdbdrinks_ratingsmodule.Test;

public class DatabaseReviewRepositoryHelperCreateUpdateReviewCommandTest
{
    private SqlConnection _connection;
    private const int TestReviewId = 1;
    private const int TestRatingId = 1;
    private const int TestUserId = 999;
    private const string TestContent = "This is an updated review";
    private static readonly DateTime TestCreationDate = DateTime.Now;
    private const bool TestIsActive = true;

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
    public void CreateUpdateReviewCommand_WhenCalled_CreatesCommandWithCorrectParameters()
    {
        // Arrange
        var review = new Review
        {
            ReviewId = TestReviewId,
            RatingId = TestRatingId,
            UserId = TestUserId,
            Content = TestContent,
            CreationDate = TestCreationDate,
            IsActive = TestIsActive
        };

        // Act
        var command = DatabaseReviewRepositoryHelper.CreateUpdateReviewCommand(_connection, review);

        // Assert
        Assert.That(command.CommandText, Is.EqualTo(ReviewQueries.UpdateReviewQuery), "Command text should match the query");
        Assert.That(command.Connection, Is.EqualTo(_connection), "Connection should be set correctly");
        
        Assert.That(command.Parameters["@ReviewId"].Value, Is.EqualTo(TestReviewId), "ReviewId parameter should be set correctly");
        Assert.That(command.Parameters["@RatingId"].Value, Is.EqualTo(TestRatingId), "RatingId parameter should be set correctly");
        Assert.That(command.Parameters["@UserId"].Value, Is.EqualTo(TestUserId), "UserId parameter should be set correctly");
        Assert.That(command.Parameters["@Content"].Value, Is.EqualTo(TestContent), "Content parameter should be set correctly");
        Assert.That(command.Parameters["@CreationDate"].Value, Is.EqualTo(TestCreationDate), "CreationDate parameter should be set correctly");
        Assert.That(command.Parameters["@IsActive"].Value, Is.EqualTo(TestIsActive), "IsActive parameter should be set correctly");
    }

    [Test]
    public void CreateUpdateReviewCommand_WhenCalledWithNullConnection_CreatesCommandButCannotExecute()
    {
        // Arrange
        var review = new Review
        {
            ReviewId = TestReviewId,
            RatingId = TestRatingId,
            UserId = TestUserId,
            Content = TestContent,
            CreationDate = TestCreationDate,
            IsActive = TestIsActive
        };

        // Act
        var command = DatabaseReviewRepositoryHelper.CreateUpdateReviewCommand(null!, review);

        // Assert
        Assert.That(command, Is.Not.Null, "Command should be created");
        Assert.That(command.Connection, Is.Null, "Command's connection should be null");
        Assert.Throws<InvalidOperationException>(() => command.ExecuteNonQuery(), 
            "Should throw InvalidOperationException when trying to execute command with null connection");
    }

    [Test]
    public void CreateUpdateReviewCommand_WhenCalledWithNullReview_ThrowsNullReferenceException()
    {
        // Act & Assert
        Assert.Throws<NullReferenceException>(() => 
            DatabaseReviewRepositoryHelper.CreateUpdateReviewCommand(_connection, null!),
            "Should throw NullReferenceException when review is null");
    }
} 