using Fluxo.Application.DTOs.FinancialMovements;
using Fluxo.Application.Interfaces.Services;
using Fluxo.ConsoleApp.Parsing;
using Fluxo.ConsoleApp.UI;
using Fluxo.Domain.Common;

namespace Fluxo.ConsoleApp.Commands;

public sealed class AddCommand
{
    private readonly IFinancialMovementService _service;
    private readonly ConsoleRenderer _renderer;

    public AddCommand(IFinancialMovementService service, ConsoleRenderer renderer)
    {
        _service = service;
        _renderer = renderer;
    }

    public async Task ExecuteAsync(CommandArguments arguments, CancellationToken cancellationToken = default)
    {
        if (arguments.Positionals.Count < 3 || !Guid.TryParse(arguments.Get("account"), out var accountId) ||
            !decimal.TryParse(arguments.Positionals[1], out var amount) ||
            !Enum.TryParse<MovementType>(arguments.Positionals[0], true, out var movementType))
        {
            _renderer.Error("Uso: add <income|expense|transfer|other> <amount> <description> --account <guid> [--currency ARS]");
            return;
        }

        var currency = Enum.TryParse<Currency>(arguments.Get("currency", "ARS"), true, out var parsedCurrency)
            ? parsedCurrency
            : Currency.Other;
        var date = DateTime.TryParse(arguments.Get("date"), out var parsedDate) ? parsedDate : DateTime.Today;
        var result = await _service.CreateAsync(new CreateFinancialMovementDto
        {
            AccountId = accountId,
            Amount = amount,
            Description = arguments.Positionals[2],
            MovementType = movementType,
            Currency = currency,
            TransactionDate = date
        }, cancellationToken);

        if (result.IsFailure)
        {
            _renderer.Error(result.Error);
            return;
        }

        _renderer.Success($"Movement created: {result.Value!.Id}");
    }
}
