using PetFamily.Domain.Shared.Models;

namespace PetFamily.API.Response.Shared;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelope
{
    private Envelope(object? result, ErrorList? errors)
    {
        Result = result;
        Errors = errors;
    }

    public object? Result { get; }
    public ErrorList? Errors { get; }
    public DateTimeOffset Execute => DateTimeOffset.UtcNow;

    public static Envelope Ok(object? result = null) =>
        new(result, null);

    public static Envelope Error(ErrorList errors) =>
        new(null, errors);
}