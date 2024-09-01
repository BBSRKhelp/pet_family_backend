using CSharpFunctionalExtensions;
using static System.String;

namespace PetFamily.Domain.Models;

public record Requisite
{
    private Requisite(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public string Title { get; }
    public string Description { get; }

    public static Result<Requisite> Create(string title, string description)
    {
        if (IsNullOrWhiteSpace(title))
            return Result.Failure<Requisite>("Title cannot be null or empty.");
        
        if (IsNullOrWhiteSpace(description))
            return Result.Failure<Requisite>("Description cannot be null or empty.");
        
        var requisite = new Requisite(title, description);
        
        return Result.Success(requisite);
    }
}