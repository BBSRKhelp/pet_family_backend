using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFamily.Core.Models;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record Email
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email, Error> Create(string email)
    {
        if (!Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"))
        {
            return Errors.General.IsInvalid(nameof(email));
        }

        return new Email(email);
    }
}