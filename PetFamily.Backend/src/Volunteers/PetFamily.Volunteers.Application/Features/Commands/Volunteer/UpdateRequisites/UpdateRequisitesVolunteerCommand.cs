using PetFamily.Core.Abstractions;
using PetFamily.Volunteers.Contracts.DTOs;

namespace PetFamily.Volunteers.Application.Features.Commands.Volunteer.UpdateRequisites;

public record UpdateRequisitesVolunteerCommand(Guid Id, IEnumerable<RequisiteDto> Requisites) : ICommand;