using PetFamily.Core.Abstractions;
using PetFamily.Core.DTOs;

namespace PetFamily.Volunteer.Application.Features.Commands.Pet.UploadFilesToPet;

public record UploadFilesToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> Files) : ICommand;