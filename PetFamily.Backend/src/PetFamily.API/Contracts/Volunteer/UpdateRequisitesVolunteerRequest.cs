using PetFamily.Application.Commands.Volunteer.UpdateRequisites;
using PetFamily.Application.Dto;

namespace PetFamily.API.Contracts.Volunteer;

public record UpdateRequisitesVolunteerRequest(IEnumerable<RequisiteDto> Requisite)
{
    public UpdateRequisitesVolunteerCommand ToCommand(Guid id)
    {
        return new UpdateRequisitesVolunteerCommand(id, Requisite);
    }
}