using PetFamily.Domain.Shared.Models;

namespace PetFamily.API.Contracts.Shared;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelope
{
    private Envelope(object? result, IEnumerable<ResponseError> errors)
    {
        Result = result;
        Errors = errors;
    }

    public object? Result { get; }
    public IEnumerable<ResponseError> Errors { get; }
    public DateTimeOffset Execute => DateTimeOffset.UtcNow;

    public static Envelope Ok(object? result = null) =>
        new(result, []);

    public static Envelope Error(IEnumerable<ResponseError> errors) =>
        new(null, errors);
}