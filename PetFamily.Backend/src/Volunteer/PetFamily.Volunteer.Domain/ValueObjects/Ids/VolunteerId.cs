using CSharpFunctionalExtensions;

namespace PetFamily.Volunteer.Domain.ValueObjects.Ids;

public class VolunteerId : ComparableValueObject
{
    private VolunteerId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }
    
    public static VolunteerId NewId() => new(Guid.NewGuid());
    public static VolunteerId Empty() => new(Guid.Empty);
    public static VolunteerId Create(Guid id) => new(id);

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator VolunteerId(Guid id) => Create(id);
    public static implicit operator Guid(VolunteerId volunteerId) => volunteerId.Value;
}
