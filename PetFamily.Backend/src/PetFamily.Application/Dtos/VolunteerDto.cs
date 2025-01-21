using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace PetFamily.Application.Dtos;

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
    
    public RequisiteDto[] Requisites { get; set; } = [];
    
    public SocialNetworkDto[] SocialNetworks { get; set; } = [];

    // public PetDto[] Pets { get; init; } = []; Надо ли?
    
    [JsonIgnore]
    public bool IsDeleted { get; init; } // Думаю надо это по другому сделать. Добавил для WHERE чтобы не вывоводило
                                         // волонтеров с IsDeleted = true
}