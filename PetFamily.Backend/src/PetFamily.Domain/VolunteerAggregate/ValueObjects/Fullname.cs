using CSharpFunctionalExtensions;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record Fullname
{
    private Fullname(string firstName, string lastName, string patronymic)
    {
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public string? Patronymic { get; }

    public static Result<Fullname> Create(
        string firstName,
        string lastName,
        string patronymic)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<Fullname>("First name cannot be empty or null");

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<Fullname>("Last name cannot be empty or null");
        
        if (string.IsNullOrWhiteSpace(patronymic))
            return Result.Failure<Fullname>("Patronymic cannot be empty or null");
        
        var fullname = new Fullname(firstName, lastName, patronymic);
        
        return Result.Success(fullname);
    }
}