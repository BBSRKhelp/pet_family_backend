using PetFamily.Core.Abstractions;
using PetFamily.Volunteer.Contracts.DTOs;

namespace PetFamily.Volunteer.Application.Features.Commands.Volunteer.UpdateRequisites;

public record UpdateRequisitesVolunteerCommand(Guid Id, IEnumerable<RequisiteDto> Requisites) : ICommand;