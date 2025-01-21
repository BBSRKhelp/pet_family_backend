using PetFamily.Application.SpeciesAggregate.Commands.Create;

namespace PetFamily.API.Contracts.Species;

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand()
    {
        return new CreateSpeciesCommand(Name);
    }
}