using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.SpeciesAggregate.Commands.Breed.AddBreed;

public record AddBreedCommand(Guid SpeciesId, string Name) : ICommand;