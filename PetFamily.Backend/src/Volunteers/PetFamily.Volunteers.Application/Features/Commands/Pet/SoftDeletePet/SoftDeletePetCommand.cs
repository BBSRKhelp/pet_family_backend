using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.SoftDeletePet;

public record SoftDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;