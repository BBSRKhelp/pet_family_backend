using PetFamily.Core.Interfaces.Abstractions;

namespace PetFamily.Species.Application.Commands.Species.Delete;

public record DeleteSpeciesCommand(Guid Id) : ICommand;