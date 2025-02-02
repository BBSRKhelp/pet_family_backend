using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.Application.DTOs.Pet;

public record AppearanceDetailsDto(Colour Colouration, float Weight, float Height);