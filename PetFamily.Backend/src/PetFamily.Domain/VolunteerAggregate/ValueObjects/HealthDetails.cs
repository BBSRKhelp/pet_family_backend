using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using static System.String;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record HealthDetails
{
    private HealthDetails(
        string healthInformation,
        bool isCastrated,
        bool isVaccinated)
    {
        HealthInformation = healthInformation;
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
    }

    public string HealthInformation { get; }
    public bool IsCastrated { get; }
    public bool IsVaccinated { get; }

    public static Result<HealthDetails> Create(
        string healthInformation,
        bool isCastrated,
        bool isVaccinated)
    {
        if (IsNullOrEmpty(healthInformation) || healthInformation.Length > Constants.MAX_MEDIUM_TEXT_LENGTH)
            return Result.Failure<HealthDetails>("Health information is invalid");
        
        var healthDetails = new HealthDetails(
            healthInformation,
            isCastrated,
            isVaccinated);
        
        return Result.Success(healthDetails);
    }
}