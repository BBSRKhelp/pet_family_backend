using CSharpFunctionalExtensions;
using static System.String;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record SocialNetwork
{
    private SocialNetwork(string title, string url)
    {
        Title = title;
        Url = url;
    }
    
    public string Title { get; init; }
    public string Url { get; init; }

    public static Result<SocialNetwork> Create(string title, string url)
    {
        if (IsNullOrWhiteSpace(title))
            return Result.Failure<SocialNetwork>("Title is required");
        
        if (IsNullOrWhiteSpace(url))
            return Result.Failure<SocialNetwork>("Url is required");
        
        var socialNetwork = new SocialNetwork(title, url);
        
        return Result.Success(socialNetwork);
    }
}