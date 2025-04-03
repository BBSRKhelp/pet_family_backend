using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Feature.Commands.Species.Create;

public record CreateSpeciesCommand(string Name) : ICommand;