namespace Fluxo.ConsoleApp.Parsing;

public sealed class CommandParser
{
    public ParsedCommand? Parse(string? input)
    {
        var tokens = Tokenize(input);
        if (tokens.Count == 0)
        {
            return null;
        }

        var positionals = new List<string>();
        var options = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        for (var index = 1; index < tokens.Count; index++)
        {
            var token = tokens[index];
            if (!token.StartsWith("--", StringComparison.Ordinal))
            {
                positionals.Add(token);
                continue;
            }

            var option = token[2..];
            var separator = option.IndexOf('=');
            if (separator >= 0)
            {
                options[option[..separator]] = option[(separator + 1)..];
            }
            else if (index + 1 < tokens.Count && !tokens[index + 1].StartsWith("--", StringComparison.Ordinal))
            {
                options[option] = tokens[++index];
            }
            else
            {
                options[option] = null;
            }
        }

        return new ParsedCommand(tokens[0].ToLowerInvariant(), new CommandArguments(positionals, options));
    }

    private static List<string> Tokenize(string? input)
    {
        var tokens = new List<string>();
        if (string.IsNullOrWhiteSpace(input))
        {
            return tokens;
        }

        var token = new System.Text.StringBuilder();
        var quoted = false;
        foreach (var character in input.Trim())
        {
            if (character == '"')
            {
                quoted = !quoted;
            }
            else if (char.IsWhiteSpace(character) && !quoted)
            {
                if (token.Length > 0)
                {
                    tokens.Add(token.ToString());
                    token.Clear();
                }
            }
            else
            {
                token.Append(character);
            }
        }

        if (token.Length > 0)
        {
            tokens.Add(token.ToString());
        }

        return tokens;
    }
}
