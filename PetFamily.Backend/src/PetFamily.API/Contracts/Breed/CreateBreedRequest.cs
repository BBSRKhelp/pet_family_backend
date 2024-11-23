using PetFamily.Application.Commands.Species.AddBreed;

namespace PetFamily.API.Contracts.Breed;

public record CreateBreedRequest(string Name)
{
    public CreateBreedCommand ToCommand(Guid id)
    {
        return new CreateBreedCommand(id, Name);
    }
}