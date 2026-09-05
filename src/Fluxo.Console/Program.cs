using Fluxo.Application.Interfaces.Persistence;
using Fluxo.Application.Interfaces.Services;
using Fluxo.Application.Services;
using Fluxo.ConsoleApp.Commands;
using Fluxo.ConsoleApp.Parsing;
using Fluxo.ConsoleApp.Shell;
using Fluxo.ConsoleApp.UI;
using Fluxo.Infrastructure.Configuration;
using Fluxo.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var databasePath = Path.Combine(AppContext.BaseDirectory, "fluxo.db");
services.AddInfrastructure($"Data Source={databasePath}");
services.AddScoped<IFinancialMovementRepository>(provider => provider.GetRequiredService<FinancialMovementRepository>());
services.AddScoped<IFinancialMovementService, FinancialMovementService>();
services.AddSingleton<CommandParser>();
services.AddSingleton<CommandHistory>();
services.AddSingleton<ConsoleRenderer>();
services.AddSingleton<TableRenderer>();
services.AddSingleton<DashboardRenderer>();
services.AddSingleton<PromptRenderer>();
services.AddTransient<AddCommand>();
services.AddTransient<ListCommand>();
services.AddTransient<SummaryCommand>();
services.AddTransient<HelpCommand>();
services.AddTransient<CommandDispatcher>();
services.AddTransient<FluxoShell>();

await using var provider = services.BuildServiceProvider();
using var scope = provider.CreateScope();
var dispatcher = scope.ServiceProvider.GetRequiredService<CommandDispatcher>();
var parser = scope.ServiceProvider.GetRequiredService<CommandParser>();

if (args.Length == 0)
{
	await scope.ServiceProvider.GetRequiredService<FluxoShell>().RunAsync();
}
else
{
	await dispatcher.DispatchAsync(parser.Parse(string.Join(' ', args)));
}
