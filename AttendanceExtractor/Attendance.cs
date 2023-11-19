namespace AttendanceExtractor;

public record Attendance(Guid Id, DateTimeOffset CreatedAt, string MissionId, long PlayerId);

public record AttendanceForPlots(Guid Id, DateTimeOffset MissionDate, string MissionId, long PlayerId);