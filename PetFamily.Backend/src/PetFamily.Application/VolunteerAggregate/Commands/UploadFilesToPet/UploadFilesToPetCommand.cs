using PetFamily.Application.Dtos;
using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.UploadFilesToPet;

public record UploadFilesToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> Files) : ICommand;