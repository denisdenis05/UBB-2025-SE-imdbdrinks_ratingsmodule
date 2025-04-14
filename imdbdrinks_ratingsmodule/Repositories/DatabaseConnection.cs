using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace imdbdrinks_ratingsmodule.Repositories
{
    /// <summary>
    /// Provides methods to establish a connection to the database.
    /// </summary>
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnection"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing the connection string.</param>
        public DatabaseConnection(IConfiguration configuration)
        {
            _connectionString = configuration["DbConnection"];
        }

        /// <summary>
        /// Creates and returns a new SQL connection using the stored connection string.
        /// </summary>
        /// <returns>A new <see cref="SqlConnection"/> instance.</returns>
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
