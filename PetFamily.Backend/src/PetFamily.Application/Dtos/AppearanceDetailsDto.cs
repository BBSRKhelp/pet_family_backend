using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.Application.Dtos;

public record AppearanceDetailsDto(Colour Colouration, float Weight, float Height);