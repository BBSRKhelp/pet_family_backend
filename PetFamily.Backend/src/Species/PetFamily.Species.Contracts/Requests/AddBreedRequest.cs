namespace PetFamily.Species.Contracts.Requests;

public record AddBreedRequest(string Name);
// {
//     public AddBreedCommand ToCommand(Guid id)
//     {
//         return new AddBreedCommand(id, Name);
//     }
// }