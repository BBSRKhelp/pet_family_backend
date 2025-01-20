using PetFamily.Application.Interfaces.Abstractions;

namespace PetFamily.Application.SpeciesAggregate.Commands.Create;

public record CreateSpeciesCommand(string Name) : ICommand;