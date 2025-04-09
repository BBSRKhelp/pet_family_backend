namespace PetFamily.Volunteer.Contracts.Requests;

public record ChangePetsPositionRequest(int NewPosition);
// {
//     public ChangePetsPositionCommand ToCommand(Guid volunteerId, Guid petId)
//     {
//         return new ChangePetsPositionCommand(volunteerId, petId, NewPosition);
//     }
// }