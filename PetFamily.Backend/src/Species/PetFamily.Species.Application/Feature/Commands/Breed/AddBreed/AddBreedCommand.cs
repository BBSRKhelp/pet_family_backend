using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Feature.Commands.Breed.AddBreed;

public record AddBreedCommand(Guid SpeciesId, string Name) : ICommand;