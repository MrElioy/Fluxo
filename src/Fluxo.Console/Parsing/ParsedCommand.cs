namespace Fluxo.ConsoleApp.Parsing;

public sealed class ParsedCommand
{
    public ParsedCommand(string name, CommandArguments arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    public string Name { get; }
    public CommandArguments Arguments { get; }
}
