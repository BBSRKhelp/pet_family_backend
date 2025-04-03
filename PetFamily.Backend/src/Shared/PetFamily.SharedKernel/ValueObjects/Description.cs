using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public class Description : ComparableValueObject
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

    protected override IEnumerable<IComparable?> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}