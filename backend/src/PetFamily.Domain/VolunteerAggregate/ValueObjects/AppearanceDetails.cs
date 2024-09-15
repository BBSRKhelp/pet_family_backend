using System.Drawing;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.SpeciesAggregate;
using PetFamily.Domain.SpeciesAggregate.Entities;
using PetFamily.Domain.SpeciesAggregate.ValueObjects.Ids;
using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record AppearanceDetails
{
    //ef core
    private AppearanceDetails()
    {
    }

    public AppearanceDetails(
        SpeciesId speciesId,
        BreedId breedId,
        Colour colouration,
        double weight,
        double height)
    {
        SpeciesId = speciesId;
        BreedId = breedId;
        Coloration = colouration;
        Weight = weight;
        Height = height;
    }

    public SpeciesId SpeciesId { get; }
    public BreedId BreedId { get; }
    public Colour Coloration { get; }
    public double Weight { get; }
    public double Height { get; }

    public static Result<AppearanceDetails> Create(
        SpeciesId speciesId,
        BreedId breedId,
        Colour colouration,
        double weight,
        double height)

    {
        if (speciesId == SpeciesId.Empty())
            return Result.Failure<AppearanceDetails>("Species id is invalid");
        if (breedId == BreedId.Empty())
            return Result.Failure<AppearanceDetails>("Breed is invalid");
        if (weight <= 0 || weight > Constants.MAX_MEDIUM_LOW_TEXT_LENGTH)
            return Result.Failure<AppearanceDetails>("Weight must be between 0 and 1000");
        if (height <= 0 || height > Constants.MAX_MEDIUM_LOW_TEXT_LENGTH)
            return Result.Failure<AppearanceDetails>("Height must be between 0 and 1000");
        
        var appearanceDetails = new AppearanceDetails(
            speciesId,
            breedId, 
            colouration, 
            weight,
            height);
        
        return Result.Success(appearanceDetails);
    }
}