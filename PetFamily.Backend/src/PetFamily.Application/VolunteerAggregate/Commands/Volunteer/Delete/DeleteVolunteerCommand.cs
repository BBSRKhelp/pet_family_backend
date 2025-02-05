using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.Volunteer.Delete;

public record DeleteVolunteerCommand(Guid Id) : ICommand;