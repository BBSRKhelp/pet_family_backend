using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.HardDeletePet;

public record HardDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;