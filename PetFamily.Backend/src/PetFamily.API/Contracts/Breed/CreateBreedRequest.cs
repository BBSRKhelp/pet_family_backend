using PetFamily.Application.SpeciesAggregate.Commands.Breed.AddBreed;

namespace PetFamily.API.Contracts.Breed;

public record CreateBreedRequest(string Name)
{
    public AddBreedCommand ToCommand(Guid id)
    {
        return new AddBreedCommand(id, Name);
    }
}