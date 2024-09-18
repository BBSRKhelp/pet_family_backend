using CSharpFunctionalExtensions;
using static System.String;

namespace PetFamily.Domain.VolunteerAggregate.ValueObjects;

public record Address
{
    private Address(
        string country, 
        string city, 
        string street, 
        string? postalcode)
    {
        Country = country;
        City = city;
        Street = street;
        Postalcode = postalcode;
    }

    public string Country { get; }
    public string City { get; }
    public string Street { get; }
    public string? Postalcode { get; }

    public static Result<Address> Create(
        string country,
        string city,
        string street,
        string? postalcode)
    {
        if (IsNullOrWhiteSpace(country))
            return Result.Failure<Address>("Country is required");

        if (IsNullOrWhiteSpace(city))
            return Result.Failure<Address>("City is required");

        if (IsNullOrWhiteSpace(street))
            return Result.Failure<Address>("Street is required");
        
        if (IsNullOrWhiteSpace(postalcode))
            return Result.Failure<Address>("Postcode is required");
        
        var address = new Address(country,
            city,
            street,
            postalcode);
        
        return Result.Success(address);
    }
}