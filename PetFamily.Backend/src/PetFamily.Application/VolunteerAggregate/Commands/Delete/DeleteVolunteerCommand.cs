using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.Delete;

public record DeleteVolunteerCommand(Guid Id) : ICommand;