using PetFamily.Core.Enums;
using PetFamily.Volunteer.Application.Features.Commands.Pet.UpdatePetStatus;

namespace PetFamily.Volunteer.Presentation.Volunteers.Requests;

public record UpdatePetStatusRequest(string Status)
{
    public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId)
    {
        var status = Enum.TryParse(Status, true, out Status statusResult)
            ? statusResult 
            : Core.Enums.Status.Unknown;
        
        return new UpdatePetStatusCommand(volunteerId, petId, status);
    }
}