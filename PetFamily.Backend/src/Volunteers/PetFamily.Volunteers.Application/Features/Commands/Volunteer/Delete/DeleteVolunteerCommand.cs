using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Features.Commands.Volunteer.Delete;

public record DeleteVolunteerCommand(Guid Id) : ICommand;