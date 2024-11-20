using PetFamily.Application.Commands.Species.Create;

namespace PetFamily.API.Contracts.Species;

public record SpeciesCreateRequest(string Name)
{
    public SpeciesCreateCommand ToCommand()
    {
        return new SpeciesCreateCommand(Name);
    }
}