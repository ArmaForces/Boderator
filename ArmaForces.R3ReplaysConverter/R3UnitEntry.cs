using System.Text.Json.Serialization;

namespace ArmaForces.R3ReplaysConverter;

public record R3UnitEntry
{
    /// <summary>
    /// In format:
    /// [SIDE] [GROUP]: [INDEX_IN_GROUP] ([PLAYER_NAME]) REMOTE
    /// </summary>
    /// <example>B Alpha 1-1: 1 (nomus) REMOTE</example>
    public string Unit { get; init; } = string.Empty;
    
    /// <summary>
    /// It should be equal to Steam UID.
    /// </summary>
    public string Id { get; init; } = string.Empty;

    public double[] Pos { get; init; } = {0, 0, 0};
    
    public double Dir { get; init; }

    public string Ico { get; init; } = string.Empty;

    /// <summary>
    /// ???
    /// </summary>
    public string Fac { get; init; } = string.Empty;

    public string Grp { get; init; } = string.Empty;

    public string Ldr { get; init; } = string.Empty;
}
