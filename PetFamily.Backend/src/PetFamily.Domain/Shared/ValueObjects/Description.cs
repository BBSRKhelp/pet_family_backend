using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Domain.Shared.ValueObjects;

public record Description
{
    private Description(string? value)
    {
        Value = value;
    }

    public string? Value { get; }

    public static Result<Description, Error> Create(string? description)
    {
        if (description?.Length > Constants.MAX_MEDIUM_TEXT_LENGTH)
            return Errors.General.MaxLengthExceeded(nameof(description));

        return new Description(description);
    }
}