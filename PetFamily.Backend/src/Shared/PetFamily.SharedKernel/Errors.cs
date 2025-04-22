namespace PetFamily.SharedKernel;

public static class Errors
{
    public static class General
    {
        public static Error IsInvalid(string? value = null) =>
            Error.Validation("value.is.invalid", $"{value ?? "value"} is invalid");

        public static Error NotFound<T>(T? name = null) where T : class
        {
            return Error.NotFound("record.not.found", $"record {(name is null ? "" : name)} not found");
        }

        public static Error IsRequired(string? value = null) =>
            Error.Validation("length.is.invalid", $"invalid {value ?? ""} length");

        public static Error IsExisted(string? value = null) =>
            Error.Conflict("record.is.existed", $"{value ?? ""} is already existed");

        public static Error IsAssociated(string? entityOne = null, string? entityTwo = null) =>
            Error.Conflict("records.is.associated",
                $"there are entities already associated" +
                $"{(entityOne is null || entityTwo is null ? "" : $". {entityOne} with {entityTwo}")}");

        public static Error MaxLengthExceeded(string? value = null) =>
            Error.Validation("record.exceeded", $"record {value ?? ""} exceeded");

        public static Error MinLengthLowered(string? value = null) =>
            Error.Validation("record.lowered", $"record {value ?? ""} lowered");
    }

    public static class User
    {
        public static Error InvalidCredentials() =>
            Error.Validation("credentials.is.invalid", "your credentials are invalid");
    }
}