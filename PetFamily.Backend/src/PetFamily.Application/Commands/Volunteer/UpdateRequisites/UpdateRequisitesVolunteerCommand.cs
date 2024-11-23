using PetFamily.Application.Dto;

namespace PetFamily.Application.Commands.Volunteer.UpdateRequisites;

public record UpdateRequisitesVolunteerCommand(Guid Id, IEnumerable<RequisiteDto> Requisites);