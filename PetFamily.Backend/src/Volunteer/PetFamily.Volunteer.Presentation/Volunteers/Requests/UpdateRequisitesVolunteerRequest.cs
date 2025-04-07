using PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateRequisites;
using PetFamily.Volunteer.Contracts.DTOs;

namespace PetFamily.Volunteer.Presentation.Volunteers.Requests;

public record UpdateRequisitesVolunteerRequest(IEnumerable<RequisiteDto> Requisite)
{
    public UpdateRequisitesVolunteerCommand ToCommand(Guid id)
    {
        return new UpdateRequisitesVolunteerCommand(id, Requisite);
    }
}