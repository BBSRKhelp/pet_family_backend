using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Core.Providers;

public record FileIdentifier(PhotoPath PhotoPath, string BucketName);