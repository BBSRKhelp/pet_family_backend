using PetFamily.Core.Interfaces.Abstractions;

namespace PetFamily.Species.Application.Commands.Species.Create;

public record CreateSpeciesCommand(string Name) : ICommand;