using System.Text.Json.Serialization;

namespace PetFamily.Volunteers.Contracts.DTOs;

public record VolunteerDto
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = null!;

    public string LastName { get; init; } = null!;

    public string? Patronymic { get; init; }

    public string? Description { get; init; }

    public byte WorkExperience { get; init; }

    public string PhoneNumber { get; init; } = null!;

    public string Email { get; init; } = null!;

    [JsonIgnore] public bool IsDeleted { get; init; }
}