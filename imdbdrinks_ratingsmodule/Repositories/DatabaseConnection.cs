// <copyright file="DatabaseConnection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule.Repositories
{
    using System;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Provides methods to establish a connection to the database.
    /// </summary>
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnection"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing the connection string.</param>
        public DatabaseConnection(IConfiguration configuration)
        {
            // Fix for CS8601: Ensure the connection string is not null by using null-coalescing operator.
            this.connectionString = configuration["DbConnection"]
                                    ?? throw new ArgumentNullException(nameof(configuration), "DbConnection cannot be null.");
        }

        /// <summary>
        /// Creates and returns a new SQL connection using the stored connection string.
        /// </summary>
        /// <returns>A new <see cref="SqlConnection"/> instance.</returns>
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(this.connectionString);
        }
    }
}
