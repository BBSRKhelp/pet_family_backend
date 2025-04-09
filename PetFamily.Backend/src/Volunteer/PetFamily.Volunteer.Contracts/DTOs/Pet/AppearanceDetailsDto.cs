using PetFamily.Core.Enums;

namespace PetFamily.Volunteer.Contracts.DTOs.Pet;

public record AppearanceDetailsDto(Colour Coloration, float Weight, float Height);