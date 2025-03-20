using PetFamily.Application.DTOs.Pet;
using PetFamily.Core.Interfaces.Abstractions;

namespace PetFamily.Application.VolunteerAggregate.Commands.Pet.UploadFilesToPet;

public record UploadFilesToPetCommand(Guid VolunteerId, Guid PetId, IEnumerable<UploadFileDto> Files) : ICommand;