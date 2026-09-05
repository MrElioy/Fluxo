namespace Fluxo.ConsoleApp.Shell;

public sealed class CommandHistory
{
    private readonly List<string> _entries = [];
    public IReadOnlyList<string> Entries => _entries;
    public void Add(string command)
    {
        if (!string.IsNullOrWhiteSpace(command))
        {
            _entries.Add(command);
        }
    }
}
