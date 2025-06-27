using PetFamily.Core.Abstractions;

namespace PetFamily.Species.Application.Features.Commands.Species.Delete;

public record DeleteSpeciesCommand(Guid Id) : ICommand;