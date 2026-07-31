using Fluxo.Infrastructure.Persistence;
using Fluxo.Infrastructure.Persistence.Repositories;
using Fluxo.Infrastructure.Persistence.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fluxo.Infrastructure.Configuration;

// Este archivo centraliza el registro de servicios de Infrastructure.
// El objetivo es que la aplicación ejecutable solo necesite invocar
// AddInfrastructure(connectionString) y no tenga que conocer los detalles
// de implementación de EF Core, SQLite ni repositorios concretos.
//
// Aquí se registran:
// - FluxoDbContext, que representa la sesión con la base.
// - el proveedor SQLite para EF Core.
// - los repositorios que encapsulan acceso a datos.
// - el inicializador de base para preparar la estructura al arrancar.
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
