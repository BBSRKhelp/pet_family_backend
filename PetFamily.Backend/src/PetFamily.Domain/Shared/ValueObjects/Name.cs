using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Models;

namespace PetFamily.Domain.Shared.ValueObjects;

public record Name
{
    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Name, Error> Create(string nickname)
    {
        if (nickname.Length > Constants.MAX_VERY_LOW_TEXT_LENGTH)
            return Errors.General.MaxLengthExceeded(nameof(nickname));

        return new Name(nickname);
    }
}