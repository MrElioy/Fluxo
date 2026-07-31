using Fluxo.Infrastructure.Persistence;
using Fluxo.Infrastructure.Persistence.Repositories;
using Fluxo.Infrastructure.Persistence.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxo.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FluxoDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<FinancialMovementRepository>();
        services.AddScoped<CategoryRepository>();
        services.AddScoped<DatabaseInitializer>();
        services.AddSingleton(new SQLiteConnectionFactory(connectionString));

        return services;
    }
}
