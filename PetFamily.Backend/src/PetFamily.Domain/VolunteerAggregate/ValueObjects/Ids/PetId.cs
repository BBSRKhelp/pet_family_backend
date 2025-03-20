using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects.Ids;

public class PetId : ComparableValueObject
{
    private PetId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static PetId NewId() => new(Guid.NewGuid());
    public static PetId Empty() => new(Guid.Empty);
    public static PetId Create(Guid id) => new(id);

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator PetId(Guid id) => Create(id);
    public static implicit operator Guid(PetId petId) => petId.Value;
}
