using System.Text.Json.Serialization;

namespace ArmaForces.R3ReplaysConverter;

public record R3Entry
{
    public long Id { get; init; }
    
    public string PlayerId { get; init; } = "0";
    
    public string Type { get; init; } = string.Empty;
    
    public string Value { get; init; } = string.Empty;
    
    public long MissionTime { get; init; }
};