using PetFamily.Application.Commands.Volunteer.UpdateRequisites;
using PetFamily.Application.Dto;

namespace PetFamily.API.Contracts.Volunteer;

public record VolunteerUpdateRequisitesRequest(IEnumerable<RequisiteDto> Requisite)
{
    public VolunteerUpdateRequisitesCommand ToCommand(Guid id)
    {
        return new VolunteerUpdateRequisitesCommand(id, Requisite);
    }
}