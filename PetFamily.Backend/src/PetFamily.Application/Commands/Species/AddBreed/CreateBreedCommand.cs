namespace PetFamily.Application.Commands.Species.AddBreed;

public record CreateBreedCommand(Guid SpeciesId, string Name);