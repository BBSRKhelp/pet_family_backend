using PetFamily.Core.Interfaces.Abstractions;

namespace PetFamily.Species.Application.Commands.Breed.AddBreed;

public record AddBreedCommand(Guid SpeciesId, string Name) : ICommand;