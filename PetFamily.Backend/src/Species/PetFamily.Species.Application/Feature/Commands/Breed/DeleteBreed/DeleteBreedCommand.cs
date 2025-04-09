using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Feature.Commands.Breed.DeleteBreed;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
