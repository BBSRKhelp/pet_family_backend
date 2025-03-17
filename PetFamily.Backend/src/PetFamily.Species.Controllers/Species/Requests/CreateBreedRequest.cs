using PetFamily.Species.Application.Commands.Breed.AddBreed;

namespace PetFamily.Species.Controllers.Species.Requests;

public record CreateBreedRequest(string Name)
{
    public AddBreedCommand ToCommand(Guid id)
    {
        return new AddBreedCommand(id, Name);
    }
}