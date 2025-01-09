using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.Commands.Volunteer.ChangePetsPosition;

public record ChangePetsPositionCommand(
    Guid VolunteerId,
    Guid PetId,
    int NewPosition);