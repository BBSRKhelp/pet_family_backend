using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Feature.Commands.Species.Delete;

public record DeleteSpeciesCommand(Guid Id) : ICommand;