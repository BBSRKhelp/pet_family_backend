using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using static System.String;

namespace PetFamily.Accounts.Domain.ValueObjects;

public record Certificate //TODO СПРОСИТЬ что сюда писать
{
    private Certificate(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public string Name { get; }
    public string Url { get; }

    public static Result<Certificate, Error> Create(string name, string url)
    {
        if (IsNullOrWhiteSpace(name))
            return Errors.General.IsRequired(nameof(name));
        
        if (name.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.MaxLengthExceeded(nameof(name));
        
        if (IsNullOrWhiteSpace(url))
            return Errors.General.IsRequired(nameof(url));

        if (name.Length > Constants.MAX_MEDIUM_TEXT_LENGTH)
            return Errors.General.MaxLengthExceeded(nameof(name));
        
        return new Certificate(name, url);
    }
}