using CSharpFunctionalExtensions;
using PetFamily.Core.Models;

namespace PetFamily.Core.ValueObjects;

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