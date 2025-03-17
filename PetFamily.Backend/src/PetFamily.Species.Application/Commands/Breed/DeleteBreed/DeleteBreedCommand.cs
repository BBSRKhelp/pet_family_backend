using PetFamily.Species.Application.Interfaces.Abstractions;

namespace PetFamily.Species.Application.Commands.Breed.DeleteBreed;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
