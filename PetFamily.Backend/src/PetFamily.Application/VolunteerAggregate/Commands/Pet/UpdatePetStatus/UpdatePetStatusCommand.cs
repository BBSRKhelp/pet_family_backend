using PetFamily.Application.Interfaces.Abstractions;
using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.UpdatePetStatus;

public record UpdatePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    Status Status) : ICommand;