using System.Text.Json.Serialization;
using PetFamily.Core.DTOs;
using PetFamily.Core.Enums;
using PetFamily.Volunteers.Contracts.DTOs.Pet;

namespace PetFamily.Volunteers.Contracts.DTOs;

public record PetDto
{
    public Guid Id { get; init; }

    public string? Name { get; init; }

    public string? Description { get; init; }

    public Colour Coloration { get; init; }

    public float Weight { get; init; }

    public float Height { get; init; }

    public string Country { get; init; } = null!;

    public string City { get; init; } = null!;

    public string Street { get; init; } = null!;

    public string? PostalCode { get; init; }

    public string PhoneNumber { get; init; } = null!;

    public DateTime BirthDate { get; init; }

    public Status Status { get; init; }

    public string HealthInformation { get; init; } = null!;

    public bool IsCastrated { get; init; }

    public bool IsVaccinated { get; init; }

    public int Position { get; init; }

    public Guid SpeciesId { get; init; }

    public Guid BreedId { get; init; }
    
    public Guid VolunteerId { get; init; }
    
    public RequisiteDto[] Requisites { get; set; } = [];

    public PetPhotoDto[] PetPhotos { get; set; } = [];

    [JsonIgnore] public bool IsDeleted { get; init; }
}