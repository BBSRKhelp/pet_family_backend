namespace PetFamily.Species.Contracts.Requests;

public record CreateBreedRequest(string Name)
{
    /*public AddBreedCommand ToCommand(Guid id)
    {
        return new AddBreedCommand(id, Name);
    }*/
}