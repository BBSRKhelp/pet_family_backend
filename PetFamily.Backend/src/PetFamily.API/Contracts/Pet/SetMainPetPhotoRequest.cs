using PetFamily.Application.VolunteerAggregate.Commands.Pet.SetMainPetPhoto;

namespace PetFamily.API.Contracts.Pet;

public record SetMainPetPhotoRequest(string PhotoPath)
{
    public SetMainPetPhotoCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new SetMainPetPhotoCommand(volunteerId, petId, PhotoPath);
    }
}