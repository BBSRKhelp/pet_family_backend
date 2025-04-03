namespace PetFamily.Species.Contracts.Requests;

public record CreateSpeciesRequest(string Name)
{
    /*public CreateSpeciesCommand ToCommand()
    {
        return new CreateSpeciesCommand(Name);
    }*/
}