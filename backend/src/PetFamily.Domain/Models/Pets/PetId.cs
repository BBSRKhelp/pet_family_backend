namespace PetFamily.Domain.Models.Pets;

public record PetId
{
    private PetId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }

    public static PetId NewPetId() => new(Guid.NewGuid());
}