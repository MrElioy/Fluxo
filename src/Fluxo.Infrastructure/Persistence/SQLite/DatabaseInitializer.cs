using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxo.Infrastructure.Persistence.SQLite;

// DatabaseInitializer prepara la base de datos cuando la aplicación arranca.
//
// Su rol principal es asegurar que la estructura de la base exista y esté en
// un estado consistente antes de empezar a persistir información.
//
// En esta primera etapa se usa para ejecutar migraciones pendientes con EF Core,
// pero más adelante puede expandirse para insertar datos semilla si hace falta.
public class DatabaseInitializer
{
    private readonly FluxoDbContext _context;

    public DatabaseInitializer(FluxoDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await _context.Database.MigrateAsync(cancellationToken);
    }
}
