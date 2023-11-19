using System.Text.Json.Serialization;

namespace ArmaForces.R3ReplaysConverter;

public enum R3EntryType
{
    [JsonPropertyName("markers")]
    markers,
    
    [JsonPropertyName("positions_infantry")]
    positions_infantry,
    
    [JsonPropertyName("player_disconnected")]
    player_disconnected,
    
    [JsonPropertyName("unit_killed")]
    unit_killed
}
