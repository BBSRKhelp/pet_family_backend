using PetFamily.Core.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.ChangePetsPosition;

public record ChangePetsPositionCommand(
    Guid VolunteerId,
    Guid PetId,
    int NewPosition) : ICommand;