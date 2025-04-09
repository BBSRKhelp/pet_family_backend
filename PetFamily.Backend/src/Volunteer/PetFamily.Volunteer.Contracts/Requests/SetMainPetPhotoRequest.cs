namespace PetFamily.Volunteer.Contracts.Requests;

public record SetMainPetPhotoRequest(string PhotoPath);
// {
//     public SetMainPetPhotoCommand ToCommand(Guid volunteerId, Guid petId)
//     {
//         return new SetMainPetPhotoCommand(volunteerId, petId, PhotoPath);
//     }
// }