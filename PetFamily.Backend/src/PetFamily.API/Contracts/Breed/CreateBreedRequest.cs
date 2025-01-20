using PetFamily.Application.SpeciesAggregate.Commands.AddBreed;

namespace PetFamily.API.Contracts.Breed;

public record CreateBreedRequest(string Name)
{
    public AddBreedCommand ToCommand(Guid id)
    {
        return new AddBreedCommand(id, Name);
    }
}