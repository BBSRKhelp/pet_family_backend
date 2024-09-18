using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record PhoneNumber
{
    public const int MAX_LENGTH = 11;
    
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; } = null!;

    public static Result<PhoneNumber> Create(string phone)
    {
        if (phone.Length != MAX_LENGTH || !Regex.IsMatch(phone, @"^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$"))
            return Result.Failure<PhoneNumber>("Invalid phone number.");

        var phoneNumber = new PhoneNumber(phone);
        
        return Result.Success(phoneNumber);
    }
}