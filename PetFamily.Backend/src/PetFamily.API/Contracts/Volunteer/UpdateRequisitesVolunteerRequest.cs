using PetFamily.Application.Dtos;
using PetFamily.Application.VolunteerAggregate.Commands.UpdateRequisites;

namespace PetFamily.API.Contracts.Volunteer;

public record UpdateRequisitesVolunteerRequest(IEnumerable<RequisiteDto> Requisite)
{
    public UpdateRequisitesVolunteerCommand ToCommand(Guid id)
    {
        return new UpdateRequisitesVolunteerCommand(id, Requisite);
    }
}