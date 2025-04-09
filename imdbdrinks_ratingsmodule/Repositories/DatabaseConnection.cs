namespace imdbdrinks_ratingsmodule.Repositories
{
    using System;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;

    public class DatabaseConnection
    {
        private readonly string connectionString;

        public DatabaseConnection(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            this.connectionString = configuration["DbConnection"]
                ?? throw new InvalidOperationException("DbConnection configuration is missing or null.");
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(this.connectionString);
        }
    }
}
