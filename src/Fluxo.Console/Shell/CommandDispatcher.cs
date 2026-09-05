using Fluxo.ConsoleApp.Commands;
using Fluxo.ConsoleApp.Parsing;
using Fluxo.ConsoleApp.UI;

namespace Fluxo.ConsoleApp.Shell;

public sealed class CommandDispatcher
{
    private readonly AddCommand _add;
    private readonly ListCommand _list;
    private readonly SummaryCommand _summary;
    private readonly HelpCommand _help;
    private readonly ConsoleRenderer _renderer;

    public CommandDispatcher(AddCommand add, ListCommand list, SummaryCommand summary, HelpCommand help, ConsoleRenderer renderer)
    {
        _add = add;
        _list = list;
        _summary = summary;
        _help = help;
        _renderer = renderer;
    }

    public async Task DispatchAsync(ParsedCommand? command, CancellationToken cancellationToken = default)
    {
        if (command is null) return;
        switch (command.Name)
        {
            case "add": await _add.ExecuteAsync(command.Arguments, cancellationToken); break;
            case "list":
            case "movements": await _list.ExecuteAsync(command.Arguments, cancellationToken); break;
            case "summary":
            case "balance": await _summary.ExecuteAsync(command.Arguments, cancellationToken); break;
            case "help": await _help.ExecuteAsync(command.Arguments, cancellationToken); break;
            default: _renderer.Error($"Comando desconocido: {command.Name}. Usa 'help'."); break;
        }
    }
}
