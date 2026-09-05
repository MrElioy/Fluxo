namespace Fluxo.ConsoleApp.UI;

public sealed class PromptRenderer
{
    public string? ReadLine() {
        Console.Write(Theme.Prompt);
        return Console.ReadLine();
    }
}
