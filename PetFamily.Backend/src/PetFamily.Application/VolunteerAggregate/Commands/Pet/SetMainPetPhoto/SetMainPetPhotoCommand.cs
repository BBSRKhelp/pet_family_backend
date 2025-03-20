using PetFamily.Core.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.SetMainPetPhoto;

public record SetMainPetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string PhotoPath) : ICommand;