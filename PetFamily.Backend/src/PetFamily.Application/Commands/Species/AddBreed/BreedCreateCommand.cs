namespace PetFamily.Application.Commands.Species.AddBreed;

public record BreedCreateCommand(Guid SpeciesId, string Name);