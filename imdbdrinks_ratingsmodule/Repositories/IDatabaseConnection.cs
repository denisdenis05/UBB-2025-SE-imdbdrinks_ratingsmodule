// <copyright file="IDatabaseConnection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule.Repositories
{
    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Interface for creating a database connection wrapping SqlConnection -> for mocking.
    /// </summary>
    internal interface IDatabaseConnection
    {
        /// <summary>
        /// Creates a new SQL connection.
        /// </summary>
        /// <returns>New unused SqlConnection</returns>
        public SqlConnection CreateConnection();
    }
}