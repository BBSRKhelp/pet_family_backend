using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.Application.Dtos;

public class PetDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public string Description { get; init; } = null!;

    public DateOnly Birthday { get; init; }

    public Colour Colouration { get; init; }

    public float Weight { get; init; }

    public float Height { get; init; }

    public StatusForHelp Status { get; init; }

    public string HealthInformation { get; init; } = null!;

    public bool IsCastrated { get; init; }

    public bool IsVaccinated { get; init; }

    public string Country { get; init; } = null!;

    public string City { get; init; } = null!;

    public string Street { get; init; } = null!;

    public string PostalCode { get; init; } = null!;

    public int Position { get; init; }

    public string PetPhotos { get; init; } = null!; //Создать dto для этого

    public string Requisites { get; init; } = null!; //Тоже самое
}