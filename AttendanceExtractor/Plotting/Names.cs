using System.Globalization;

namespace AttendanceExtractor.Plotting;

public static class Names
{
    public static string GetPolishWeekdayName(DayOfWeek dayOfWeek) =>
        dayOfWeek switch
        {
            DayOfWeek.Sunday => "Niedziela",
            DayOfWeek.Monday => "Poniedziałek",
            DayOfWeek.Tuesday => "Wtorek",
            DayOfWeek.Wednesday => "Środa",
            DayOfWeek.Thursday => "Czwartek",
            DayOfWeek.Friday => "Piątek",
            DayOfWeek.Saturday => "Sobota",
            _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
        };

    public static string GetPolishMonthName(int monthNumber) =>
        DateTimeFormatInfo.CurrentInfo.GetMonthName(monthNumber);
}
