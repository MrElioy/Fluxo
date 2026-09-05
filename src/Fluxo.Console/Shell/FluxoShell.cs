using Fluxo.ConsoleApp.Parsing;
using Fluxo.ConsoleApp.UI;

namespace Fluxo.ConsoleApp.Shell;

public sealed class FluxoShell
{
    private readonly CommandParser _parser;
    private readonly CommandDispatcher _dispatcher;
    private readonly CommandHistory _history;
    private readonly ConsoleRenderer _renderer;
    private readonly PromptRenderer _prompt;

    public FluxoShell(CommandParser parser, CommandDispatcher dispatcher, CommandHistory history, ConsoleRenderer renderer, PromptRenderer prompt)
    {
        _parser = parser;
        _dispatcher = dispatcher;
        _history = history;
        _renderer = renderer;
        _prompt = prompt;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        _renderer.Title();
        await _dispatcher.DispatchAsync(_parser.Parse("help"), cancellationToken);
        while (!cancellationToken.IsCancellationRequested)
        {
            var input = _prompt.ReadLine();
            if (input is null || input.Equals("exit", StringComparison.OrdinalIgnoreCase) || input.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _history.Add(input);
            await _dispatcher.DispatchAsync(_parser.Parse(input), cancellationToken);
        }
    }
}
