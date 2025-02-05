using PetFamily.Application.VolunteerAggregate.Commands.Pet.UpdatePetStatus;
using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.API.Contracts.Pet;

public record UpdatePetStatusRequest(string Status)
{
    public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId)
    {
        var status = Enum.Parse<Status>(Status, true);
        
        return new UpdatePetStatusCommand(volunteerId, petId, status);
    }
}