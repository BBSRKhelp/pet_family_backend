using PetFamily.Application.Dto;

namespace PetFamily.Application.Commands.Volunteer.UploadFilesToPet;

public record UploadFilesToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<CreateFileDto> Files);