using PetFamily.Application.Dto;

namespace PetFamily.Application.Commands.Volunteer.UpdateRequisites;

public record VolunteerUpdateRequisitesCommand(Guid Id, IEnumerable<RequisiteDto> Requisites);