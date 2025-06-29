using System.Text.Json.Serialization;

namespace PetFamily.SharedKernel;

public record Error
{
    private const string SEPARATOR = "||";

    private Error(string code, string message, ErrorType type, string? invalidField = null)
    {
        Code = code;
        Message = message;
        InvalidField = invalidField;
        Type = type;
    }

    public string Code { get; }
    public string Message { get; }
    public string? InvalidField { get; }
    [JsonIgnore] public ErrorType Type { get; }

    public static Error Validation(string code, string message, string? invalidField = null) =>
        new Error(code, message, ErrorType.Validation, invalidField);

    public static Error NotFound(string code, string message) =>
        new Error(code, message, ErrorType.NotFound);

    public static Error Failure(string code, string message) =>
        new Error(code, message, ErrorType.Failure);

    public static Error Conflict(string code, string message) =>
        new Error(code, message, ErrorType.Conflict);
    
    public static Error Unauthorized(string code, string message) =>
        new Error(code, message, ErrorType.Unauthorized);

    public string Serialize()
    {
        return string.Join(SEPARATOR, Code, Message, Type);
    }

    public static Error Deserialize(string serialized)
    {
        var parts = serialized.Split(SEPARATOR);

        if (parts.Length != 3)
            throw new ArgumentException("Invalid serialized format");

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
            throw new ArgumentException("Invalid serialized format");

        return new Error(parts[0], parts[1], type);
    }
}