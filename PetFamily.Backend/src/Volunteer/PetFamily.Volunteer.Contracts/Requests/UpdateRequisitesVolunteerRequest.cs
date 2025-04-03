using PetFamily.Volunteer.Contracts.DTOs;

namespace PetFamily.Volunteer.Contracts.Requests;

public record UpdateRequisitesVolunteerRequest(IEnumerable<RequisiteDto> Requisite)
{
    /*public UpdateRequisitesVolunteerCommand ToCommand(Guid id)
    {
        return new UpdateRequisitesVolunteerCommand(id, Requisite);
    }*/
}