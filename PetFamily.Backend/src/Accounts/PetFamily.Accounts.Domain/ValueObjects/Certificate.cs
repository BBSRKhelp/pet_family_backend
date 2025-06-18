using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;
using static System.String;

namespace PetFamily.Accounts.Domain.ValueObjects;

public record Certificate
{
    private Certificate(string name, string url, DateOnly issueDate)
    {
        Name = name;
        Url = url;
        IssueDate = issueDate;
    }

    public string Name { get; }
    public string Url { get; }
    public DateOnly IssueDate { get; }

    public static Result<Certificate, Error> Create(string name, string url, DateOnly issueDate)
    {
        if (IsNullOrWhiteSpace(name))
            return Errors.General.IsRequired(nameof(name));
        
        if (name.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.MaxLengthExceeded(nameof(name));
        
        if (IsNullOrWhiteSpace(url))
            return Errors.General.IsRequired(nameof(url));

        if (name.Length > Constants.MAX_MEDIUM_TEXT_LENGTH)
            return Errors.General.MaxLengthExceeded(nameof(name));
        
        if (issueDate.Year < 2020 || issueDate.ToDateTime(TimeOnly.MinValue) > DateTime.Today)
            return Errors.General.IsInvalid(nameof(issueDate));
        
        return new Certificate(name, url,  issueDate);
    }
}