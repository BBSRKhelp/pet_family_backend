using CSharpFunctionalExtensions;
using PetFamily.Core.Models;

namespace PetFamily.Core.ValueObjects;

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