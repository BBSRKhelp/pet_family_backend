using PetFamily.Core.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.HardDeletePet;

public record HardDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;