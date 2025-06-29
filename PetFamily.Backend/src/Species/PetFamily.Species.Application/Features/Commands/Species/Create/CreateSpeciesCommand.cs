using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Features.Commands.Species.Create;

public record CreateSpeciesCommand(string Name) : ICommand;