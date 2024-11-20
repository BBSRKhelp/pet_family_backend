using PetFamily.Application.Commands.Species.AddBreed;

namespace PetFamily.API.Contracts.Breed;

public record BreedCreateRequest(string Name)
{
    public BreedCreateCommand ToCommand(Guid id)
    {
        return new BreedCreateCommand(id, Name);
    }
}