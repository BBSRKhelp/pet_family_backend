using PetFamily.Domain.VolunteerAggregate.Enums;

namespace PetFamily.Application.Dto;

public record AppearanceDetailsDto(Colour Colouration, float Weight, float Height);