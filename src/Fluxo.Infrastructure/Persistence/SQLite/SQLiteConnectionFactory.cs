using Microsoft.Data.Sqlite;

namespace Fluxo.Infrastructure.Persistence.SQLite;

public class SQLiteConnectionFactory
{
    private readonly string _connectionString;

    public SQLiteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}
