namespace Fluxo.Application.Results;

/// <summary>
/// Describes an expected Application failure through a stable code,
/// a human-readable message and a general error classification.
/// </summary>

public sealed class Error
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error None => new("NONE", string.Empty);
}
