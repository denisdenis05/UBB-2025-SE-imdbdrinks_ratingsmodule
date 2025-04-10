namespace imdbdrinks_ratingsmodule.Repositories
{
    using System;
    using imdbdrinks_ratingsmodule.Constants.ErrorMessages;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;

    public class DatabaseConnection
    {
        private readonly string connectionString;

        public DatabaseConnection(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            this.connectionString = configuration["DbConnection"]
                ?? throw new InvalidOperationException(DatabaseConnectionErrorMessages.ConnectionStringNotFound);
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(this.connectionString);
        }
    }
}
