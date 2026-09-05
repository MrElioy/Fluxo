namespace Fluxo.ConsoleApp.Parsing;

public sealed class CommandArguments
{
    private readonly IReadOnlyList<string> _positionals;
    private readonly IReadOnlyDictionary<string, string?> _options;

    public CommandArguments(IReadOnlyList<string> positionals, IReadOnlyDictionary<string, string?> options)
    {
        _positionals = positionals;
        _options = options;
    }

    public IReadOnlyList<string> Positionals => _positionals;
    public string? this[string name] => _options.TryGetValue(name, out var value) ? value : null;
    public bool Has(string name) => _options.ContainsKey(name);
    public string? Get(string name, string? fallback = null) => this[name] ?? fallback;
}
