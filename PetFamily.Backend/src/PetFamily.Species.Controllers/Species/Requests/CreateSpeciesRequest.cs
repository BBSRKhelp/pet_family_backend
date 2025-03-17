using PetFamily.Species.Application.Commands.Species.Create;

namespace PetFamily.Species.Controllers.Species.Requests;

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand()
    {
        return new CreateSpeciesCommand(Name);
    }
}