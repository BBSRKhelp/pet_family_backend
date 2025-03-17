using PetFamily.Species.Application.Interfaces.Abstractions;

namespace PetFamily.Species.Application.Commands.Breed.AddBreed;

public record AddBreedCommand(Guid SpeciesId, string Name) : ICommand;