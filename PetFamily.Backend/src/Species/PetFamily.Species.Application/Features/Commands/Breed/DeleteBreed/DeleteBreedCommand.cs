using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Features.Commands.Breed.DeleteBreed;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
