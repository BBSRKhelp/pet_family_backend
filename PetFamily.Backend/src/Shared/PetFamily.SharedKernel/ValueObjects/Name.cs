using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public class Name : ComparableValueObject
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

    protected override IEnumerable<IComparable?> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}