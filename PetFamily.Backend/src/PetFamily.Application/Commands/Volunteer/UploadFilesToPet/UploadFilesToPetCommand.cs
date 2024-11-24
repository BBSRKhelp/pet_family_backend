using PetFamily.Application.Dtos;

namespace PetFamily.Application.Commands.Volunteer.UploadFilesToPet;

public record UploadFilesToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> Files);