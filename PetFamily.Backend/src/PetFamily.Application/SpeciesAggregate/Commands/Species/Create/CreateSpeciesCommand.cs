using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.SpeciesAggregate.Commands.Species.Create;

public record CreateSpeciesCommand(string Name) : ICommand;