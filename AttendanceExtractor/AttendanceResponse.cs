namespace AttendanceExtractor;

public record AttendanceResponse(IEnumerable<Attendance> Data, int Items, int TotalItems, int CurrentPage, int LastPage, int ItemsPerPage);