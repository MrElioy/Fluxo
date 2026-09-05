using Fluxo.Application.DTOs.FinancialMovements;
using Fluxo.Domain.Common;

namespace Fluxo.ConsoleApp.UI;

public sealed class DashboardRenderer
{
    public void Render(IEnumerable<FinancialMovementDto> movements)
    {
        var items = movements.ToList();
        var income = items.Where(x => x.MovementType == MovementType.Income).Sum(x => x.Amount);
        var expenses = items.Where(x => x.MovementType == MovementType.Expense).Sum(x => x.Amount);
        Console.WriteLine($"Income:  {income:N2}");
        Console.WriteLine($"Expenses: {expenses:N2}");
        Console.WriteLine($"Balance: {income - expenses:N2}");
        Console.WriteLine($"Movements: {items.Count}");
    }
}
