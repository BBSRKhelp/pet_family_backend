using PetFamily.Species.Application.Interfaces.Abstractions;

namespace PetFamily.Species.Application.Commands.Species.Delete;

public record DeleteSpeciesCommand(Guid Id) : ICommand;