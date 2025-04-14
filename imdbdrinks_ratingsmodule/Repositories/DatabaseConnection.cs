using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace imdbdrinks_ratingsmodule.Repositories;

public class DatabaseConnection : IDatabaseConnection
{
    private readonly string _connectionString;

    public DatabaseConnection(IConfiguration configuration)
    {
        _connectionString = configuration["DbConnection"];
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
