namespace AttendanceExtractor.Plotting.Data;

public record StatsForWeekday
{
    public DayOfWeek DayOfWeek { get; init; }
    public double AveragePlayersCount { get; init; }
    public int MissionsCount { get; init; }
}

public record StatsForMonth
{
    public int Year { get; init; }
    public int Month { get; init; } 
    // public double AveragePlayersCount { get; init; }
    public double UniquePlayersCount { get; init; }
    public int MissionsCount { get; init; }
}

public record StatsForYear
{
    public int Year { get; init; }
    public double UniquePlayersCount { get; init; }
    public int MissionsCount { get; init; }
}