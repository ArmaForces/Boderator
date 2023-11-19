namespace ArmaForces.R3ReplaysConverter;

public record MissionAttendanceData
{
    public string ReplayFilePath { get; init; }
    public string MapCode { get; init; }
    public DateTimeOffset MissionDate { get; init; }
    public string MissionName { get; init; }
    public List<PlayerInfo> Players { get; init; }

    public long MissionId => MissionDate.ToUnixTimeSeconds();
}
