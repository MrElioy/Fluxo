using Fluxo.Application.Interfaces.Services;
using Fluxo.ConsoleApp.Parsing;
using Fluxo.ConsoleApp.UI;

namespace Fluxo.ConsoleApp.Commands;

public sealed class SummaryCommand
{
    private readonly IFinancialMovementService _service;
    private readonly DashboardRenderer _dashboard;
    private readonly ConsoleRenderer _renderer;

    public SummaryCommand(IFinancialMovementService service, DashboardRenderer dashboard, ConsoleRenderer renderer)
    {
        _service = service;
        _dashboard = dashboard;
        _renderer = renderer;
    }

    public async Task ExecuteAsync(CommandArguments arguments, CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(cancellationToken: cancellationToken);
        if (result.IsFailure)
        {
            _renderer.Error(result.Error);
            return;
        }

        _dashboard.Render(result.Value ?? []);
    }
}
