using Microsoft.Data.Sqlite;

namespace Fluxo.Infrastructure.Persistence.SQLite;

// SQLiteConnectionFactory encapsula la creación de conexiones SQLite.
//
// Este tipo sirve como punto central para construir instancias de SqliteConnection
// con la cadena de conexión que la aplicación configure.
//
// En esta versión es una implementación simple, pero deja preparado el proyecto
// para centralizar toda la creación de conexiones si en el futuro se necesitan
// conexiones manuales o ajustes específicos del motor.
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
