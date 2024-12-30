using PetFamily.Domain.VolunteerAggregate.ValueObjects;

namespace PetFamily.Application.Providers;

public record FileIdentifier(PhotoPath PhotoPath, string BucketName);