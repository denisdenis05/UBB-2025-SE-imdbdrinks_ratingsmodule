using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using imdbdrinks_ratingsmodule.Repositories;

namespace imdbdrinks_ratingsmodule.Test.Helpers
{
    public class TestDatabaseHelper
    {
        private readonly DatabaseConnection _connection;
        private readonly IConfiguration _configuration;

        public TestDatabaseHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new DatabaseConnection(configuration);
        }


        public static IConfiguration CreateTestConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }


        private void ResetTestDatabase()
        {
            SqlConnection sqlConnecton = _connection.CreateConnection();

            try
            {
                sqlConnecton.Open();

                // Clear all tables
                ClearTable("Ratings", sqlConnecton);
                ClearTable("Reviews", sqlConnecton);


            }
            finally
            {
                sqlConnecton.Close();
            }
        }


        private void ClearTable(string tableName,SqlConnection sqlConnecton)
        {
            try
            {
                ExecuteNonQuery($"DELETE FROM {tableName}", sqlConnecton);

                // Reset the identity counter to 0 (SQL Server will start with 1)
                ExecuteNonQuery($"IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = '{tableName}') " +
                             $"DBCC CHECKIDENT ('{tableName}', RESEED, 0)", sqlConnecton);
            }
            catch (Exception)
            {
                // If table doesn't exist, just continue
            }
        }


        private int ExecuteNonQuery(string commandText,SqlConnection sqlConnection)
        {
            using SqlCommand command = new SqlCommand(commandText, sqlConnection);
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected;
        }

        private void CreateTestData()
        {
            SqlConnection sqlConnecton = _connection.CreateConnection();

            try
            {
                sqlConnecton.Open();
                int rowsAffected = ExecuteNonQuery(@"INSERT INTO ratings " +
                    "(ProductID, UserID, RatingValue, RatingDate, IsActive)" +
                    "VALUES (101, 1, 4, '2025-04-01 10:30:00', 1)," +
                    "(102, 2, 3, '2025-04-02 12:00:00', 1)," +
                    "(103, 3, 5, '2025-04-03 14:15:00', 1);",sqlConnecton);
                Console.WriteLine($"Inserted {rowsAffected} rows into Ratings table.");
            }
            finally
            {
                sqlConnecton.Close();
            }
        }


        public void PrepareTestDatabase()
        {
            ResetTestDatabase();
            CreateTestData();
        }
    }
}