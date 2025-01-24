using PetFamily.Application.DTOs;
using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.UpdateRequisites;

public record UpdateRequisitesVolunteerCommand(Guid Id, IEnumerable<RequisiteDto> Requisites) : ICommand;