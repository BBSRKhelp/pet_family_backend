using CSharpFunctionalExtensions;
using static System.String;

namespace PetFamily.Domain.Models.Pets;

public record Address
{
    private Address(
        string country, 
        string city, 
        string street, 
        string? postcode)
    {
        Country = country;
        City = city;
        Street = street;
        Postcode = postcode;
    }

    public string Country { get; }
    public string City { get; }
    public string Street { get; }
    public string? Postcode { get; }

    public static Result<Address> Create(
        string country,
        string city,
        string street,
        string? postcode)
    {
        if (IsNullOrWhiteSpace(country))
            return Result.Failure<Address>("Country is required");

        if (IsNullOrWhiteSpace(city))
            return Result.Failure<Address>("City is required");

        if (IsNullOrWhiteSpace(street))
            return Result.Failure<Address>("Street is required");
        
        if (IsNullOrWhiteSpace(postcode))
            return Result.Failure<Address>("Postcode is required");
        
        var address = new Address(country,
            city,
            street,
            postcode);
        
        return Result.Success(address);
    }
}