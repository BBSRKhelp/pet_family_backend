using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Domain.Shared.ValueObjects;

public record Name
{
    private Name(string? value)
    {
        Value = value;
    }

    public string? Value { get; }

    public static Result<Name, Error> Create(string? name)
    {
        if (name?.Length > Constants.MAX_VERY_LOW_TEXT_LENGTH)
            return Errors.General.MaxLengthExceeded(nameof(name));

        return new Name(name);
    }
}