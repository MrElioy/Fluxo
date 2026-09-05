using Fluxo.Application.DTOs.FinancialMovements;

namespace Fluxo.ConsoleApp.UI;

public sealed class TableRenderer
{
    public void Render(IEnumerable<FinancialMovementDto> movements)
    {
        Console.WriteLine("DATE\tTYPE\tAMOUNT\tCURRENCY\tDESCRIPTION");
        foreach (var movement in movements)
        {
            Console.WriteLine($"{movement.TransactionDate:yyyy-MM-dd}\t{movement.MovementType}\t{movement.Amount:N2}\t{movement.Currency}\t{movement.Description}");
        }
    }
}
