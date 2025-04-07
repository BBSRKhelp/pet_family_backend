using PetFamily.Species.Application.Feature.Commands.Breed.AddBreed;

namespace PetFamily.Species.Presentation.Species.Requests;

public record CreateBreedRequest(string Name)
{
    public AddBreedCommand ToCommand(Guid id)
    {
        return new AddBreedCommand(id, Name);
    }
}