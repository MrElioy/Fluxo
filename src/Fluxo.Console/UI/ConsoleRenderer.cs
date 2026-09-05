using Fluxo.Application.Results;

namespace Fluxo.ConsoleApp.UI;

public sealed class ConsoleRenderer
{
    public void Title() => Console.WriteLine($"\n{Theme.Title}\n");
    public void Success(string message) => Console.WriteLine($"{Theme.Success} {message}");
    public void Error(string message) => Console.Error.WriteLine($"{Theme.Error} {message}");
    public void Error(Error? error) => Error(error?.Message ?? "Ocurrió un error.");
    public void Info(string message) => Console.WriteLine(message);
}
