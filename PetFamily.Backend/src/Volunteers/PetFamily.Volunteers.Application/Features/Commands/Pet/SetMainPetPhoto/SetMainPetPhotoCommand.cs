using PetFamily.Core.Abstractions;

namespace PetFamily.Volunteers.Application.Features.Commands.Pet.SetMainPetPhoto;

public record SetMainPetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    string PhotoPath) : ICommand;