using PetFamily.Volunteer.Application.Features.Commands.Pet.SetMainPetPhoto;

namespace PetFamily.Volunteer.Presentation.Volunteers.Requests;

public record SetMainPetPhotoRequest(string PhotoPath)
{
    public SetMainPetPhotoCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new SetMainPetPhotoCommand(volunteerId, petId, PhotoPath);
    }
}