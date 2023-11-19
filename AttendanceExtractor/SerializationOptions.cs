using System.Text.Json;

namespace AttendanceExtractor;

public static class SerializationOptions
{
    public static JsonSerializerOptions Json { get; } = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}
