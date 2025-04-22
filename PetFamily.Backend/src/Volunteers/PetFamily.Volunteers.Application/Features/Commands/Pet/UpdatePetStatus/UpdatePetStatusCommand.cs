using PetFamily.Core.Abstractions;
using PetFamily.Core.Enums;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.UpdatePetStatus;

public record UpdatePetStatusCommand(
    Guid VolunteerId,
    Guid PetId,
    Status Status) : ICommand;