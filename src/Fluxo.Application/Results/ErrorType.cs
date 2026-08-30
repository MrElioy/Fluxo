namespace Fluxo.Application.Results;

/// <summary>
/// Identifies the general kind of an expected Application failure so that
/// presentation clients can decide how to represent it without inspecting
/// exception messages or depending on Infrastructure details.
/// </summary>

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Failure,
    Unexpected
}
