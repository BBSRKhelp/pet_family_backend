using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Contracts.Requests;

public record UpdateRequisitesVolunteerRequest(IEnumerable<RequisiteDto> Requisite);