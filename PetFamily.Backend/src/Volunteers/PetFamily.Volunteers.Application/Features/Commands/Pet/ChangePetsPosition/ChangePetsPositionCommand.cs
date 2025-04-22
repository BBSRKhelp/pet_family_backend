using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.ChangePetsPosition;

public record ChangePetsPositionCommand(
    Guid VolunteerId,
    Guid PetId,
    int NewPosition) : ICommand;