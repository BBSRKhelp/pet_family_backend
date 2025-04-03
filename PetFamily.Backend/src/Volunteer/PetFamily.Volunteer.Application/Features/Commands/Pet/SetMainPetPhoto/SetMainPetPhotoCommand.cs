using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteer.Application.Features.Commands.Pet.SetMainPetPhoto;

public record SetMainPetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string PhotoPath) : ICommand;