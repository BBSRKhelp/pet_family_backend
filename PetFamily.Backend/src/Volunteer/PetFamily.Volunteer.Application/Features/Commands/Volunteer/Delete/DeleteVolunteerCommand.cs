using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteer.Application.Features.Commands.Volunteer.Delete;

public record DeleteVolunteerCommand(Guid Id) : ICommand;