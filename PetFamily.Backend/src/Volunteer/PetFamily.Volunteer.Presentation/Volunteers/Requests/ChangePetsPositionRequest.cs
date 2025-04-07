using PetFamily.Volunteer.Application.Features.Commands.Pet.ChangePetsPosition;

namespace PetFamily.Volunteer.Presentation.Volunteers.Requests;

public record ChangePetsPositionRequest(int NewPosition)
{
    public ChangePetsPositionCommand ToCommand(Guid volunteerId, Guid petId)
    {
        return new ChangePetsPositionCommand(volunteerId, petId, NewPosition);
    }
}