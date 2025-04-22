using PetFamily.Core.Enums;

namespace PetFamily.Volunteers.Contracts.DTOs.Pet;

public record AppearanceDetailsDto(Colour Coloration, float Weight, float Height);