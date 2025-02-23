using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.SpeciesAggregate.Commands.Species.Delete;

public record DeleteSpeciesCommand(Guid Id) : ICommand;