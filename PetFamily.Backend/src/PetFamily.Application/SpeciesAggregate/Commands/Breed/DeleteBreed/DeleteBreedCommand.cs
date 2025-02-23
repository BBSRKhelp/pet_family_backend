using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.SpeciesAggregate.Commands.Breed.DeleteBreed;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;
