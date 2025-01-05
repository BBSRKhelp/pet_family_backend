using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.Providers;

public record FileData(Stream Stream, FileIdentifier FileIdentifier);