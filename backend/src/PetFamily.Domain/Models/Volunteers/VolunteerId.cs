using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models.Volunteers;

public record VolunteerId
{
    private VolunteerId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }
    
    public static VolunteerId NewVolunteerId() => new(Guid.NewGuid());
}