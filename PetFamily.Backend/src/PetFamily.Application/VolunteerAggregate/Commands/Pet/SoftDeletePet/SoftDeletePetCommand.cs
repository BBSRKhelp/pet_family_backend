using PetFamily.Core.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.SoftDeletePet;

public record SoftDeletePetCommand(Guid VolunteerId, Guid PetId) : ICommand;