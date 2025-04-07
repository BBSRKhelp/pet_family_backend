using PetFamily.Species.Application.Feature.Commands.Species.Create;

namespace PetFamily.Species.Presentation.Species.Requests;

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand()
    {
        return new CreateSpeciesCommand(Name);
    }
}