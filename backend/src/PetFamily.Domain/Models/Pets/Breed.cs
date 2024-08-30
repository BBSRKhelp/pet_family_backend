using CSharpFunctionalExtensions;
using static System.String;

namespace PetFamily.Domain.Models.Pets;

public class Breed
{
    private Breed(string breedPet)
    {
        BreedPet = breedPet;
    }

    public Guid Id { get; set; }
    public string BreedPet { get; }

    public static Result<Breed> Create(string breedPet)
    {
        if (IsNullOrWhiteSpace(breedPet))
            return Result.Failure<Breed>("BreedPet cannot be null or empty.");
        
        var breed = new Breed(breedPet);

        return Result.Success(breed);
    }
}