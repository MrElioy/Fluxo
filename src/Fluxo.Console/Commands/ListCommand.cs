using Fluxo.Application.DTOs.FinancialMovements;
using Fluxo.Application.Interfaces.Services;
using Fluxo.ConsoleApp.Parsing;
using Fluxo.ConsoleApp.UI;
using Fluxo.Domain.Common;

namespace Fluxo.ConsoleApp.Commands;

public sealed class ListCommand
{
    private readonly IFinancialMovementService _service;
    private readonly TableRenderer _table;
    private readonly ConsoleRenderer _renderer;

    public ListCommand(IFinancialMovementService service, TableRenderer table, ConsoleRenderer renderer)
    {
        _service = service;
        _table = table;
        _renderer = renderer;
    }

    public async Task ExecuteAsync(CommandArguments arguments, CancellationToken cancellationToken = default)
    {
        var filter = new FinancialMovementFilterDto
        {
            AccountId = Guid.TryParse(arguments.Get("account"), out var accountId) ? accountId : null,
            MovementType = Enum.TryParse<MovementType>(arguments.Get("type"), true, out var movementType) ? movementType : null,
            Currency = Enum.TryParse<Currency>(arguments.Get("currency"), true, out var currency) ? currency : null
        };
        var result = await _service.GetAllAsync(filter, cancellationToken);
        if (result.IsFailure)
        {
            _renderer.Error(result.Error);
            return;
        }

        var movements = result.Value ?? [];
        if (int.TryParse(arguments.Get("last"), out var last) && last > 0)
        {
            movements = movements.OrderByDescending(x => x.TransactionDate).Take(last).ToList();
        }
        _table.Render(movements);
    }
}
