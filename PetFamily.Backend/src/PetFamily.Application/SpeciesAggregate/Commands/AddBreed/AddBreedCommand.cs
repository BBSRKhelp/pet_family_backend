using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.SpeciesAggregate.Commands.AddBreed;

public record AddBreedCommand(Guid SpeciesId, string Name) : ICommand;