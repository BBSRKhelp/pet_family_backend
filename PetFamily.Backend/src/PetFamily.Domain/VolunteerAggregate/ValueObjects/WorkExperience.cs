using CSharpFunctionalExtensions;
using PetFamily.Core.Models;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record WorkExperience
{
    public const int MAX_NUMBER = 100;

    private WorkExperience(byte value)
    {
        Value = value;
    }

    public byte Value { get; }

    public static Result<WorkExperience, Error> Create(byte workExperience)
    {
        if (workExperience > MAX_NUMBER)
            return Errors.General.MaxLengthExceeded(nameof(workExperience));

        return new WorkExperience(workExperience);
    }
}