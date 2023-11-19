namespace ArmaForces.R3ReplaysConverter;

public record PlayerInfo
{
    public string Name { get; init; } = string.Empty;
    
    public ulong SteamUid { get; init; }

    public string[] OtherNames { get; init; } = Array.Empty<string>();
}
