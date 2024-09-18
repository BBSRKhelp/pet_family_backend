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
    
    private AppearanceDetails(
        Colour colouration,
        double weight,
        double height)
    {
        Coloration = colouration;
        Weight = weight;
        Height = height;
    }
    
    public Colour Coloration { get; }
    public double Weight { get; }
    public double Height { get; }

    public static Result<AppearanceDetails> Create(
        Colour colouration,
        double weight,
        double height)

    {
        if (weight is <= 0 or > Constants.MAX_MEDIUM_LOW_TEXT_LENGTH)
            return Result.Failure<AppearanceDetails>("Weight must be between 0 and 1000");
        if (height is <= 0 or > Constants.MAX_MEDIUM_LOW_TEXT_LENGTH)
            return Result.Failure<AppearanceDetails>("Height must be between 0 and 1000");
        
        var appearanceDetails = new AppearanceDetails(
            colouration, 
            weight,
            height);
        
        return Result.Success(appearanceDetails);
    }
}