using PetFamily.Core.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.Volunteer.Delete;

public record DeleteVolunteerCommand(Guid Id) : ICommand;