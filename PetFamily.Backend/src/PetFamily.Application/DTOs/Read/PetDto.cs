using PetFamily.Application.DTOs.Pet;
using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.Application.DTOs.Read;

public record PetDto
{
    public Guid Id { get; init; }

    public string? Name { get; init; } = null!;

    public string? Description { get; init; } = null!;

    public Colour Colouration { get; init; }

    public float Weight { get; init; }

    public float Height { get; init; }

    public string Country { get; init; } = null!;

    public string City { get; init; } = null!;

    public string Street { get; init; } = null!;

    public string? PostalCode { get; init; } = null!;

    public string PhoneNumber { get; init; } = null!;

    public DateOnly Birthday { get; init; }

    public Status Status { get; init; }

    public string HealthInformation { get; init; } = null!;

    public bool IsCastrated { get; init; }

    public bool IsVaccinated { get; init; }

    public RequisiteDto[] Requisites { get; init; } = [];

    public PetPhotoDto[] PetPhotos { get; init; } = [];

    public int Position { get; init; }

    public Guid SpeciesId { get; init; }

    public Guid BreedId { get; init; }
}