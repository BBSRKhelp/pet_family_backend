using System.Text.Json.Serialization;

namespace PetFamily.Volunteers.Contracts.DTOs;

public record VolunteerDto
{
    public Guid Id { get; init; }

    [JsonIgnore] public bool IsDeleted { get; init; }
}