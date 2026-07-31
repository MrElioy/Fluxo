using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxo.Infrastructure.Persistence.SQLite;

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
