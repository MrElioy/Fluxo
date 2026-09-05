using Fluxo.ConsoleApp.Parsing;
using Fluxo.ConsoleApp.UI;

namespace Fluxo.ConsoleApp.Commands;

public sealed class HelpCommand
{
    private readonly ConsoleRenderer _renderer;

    public HelpCommand(ConsoleRenderer renderer) => _renderer = renderer;

    public Task ExecuteAsync(CommandArguments arguments, CancellationToken cancellationToken = default)
    {
        _renderer.Info("""
            Commands:
              add <type> <amount> <description> --account <guid> [--currency ARS] [--date yyyy-MM-dd]
              list [--account <guid>] [--type income|expense|transfer|other] [--last N]
              summary
              help
              exit
            """);
        return Task.CompletedTask;
    }
}
