using CSharpFunctionalExtensions;
using static System.String;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record HealthDetails
{
    //ef core
    private HealthDetails()
    {
    }

    private HealthDetails(string healthInformation,
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
        if (IsNullOrEmpty(healthInformation))
            return Result.Failure<HealthDetails>($"'{nameof(HealthInformation)}' cannot be null or empty.");
        
        var healthDetails = new HealthDetails(
            healthInformation,
            isCastrated,
            isVaccinated);
        
        return Result.Success(healthDetails);
    }
}