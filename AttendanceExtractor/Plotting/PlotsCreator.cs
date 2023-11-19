using AttendanceExtractor.Plotting.Data;
using AttendanceExtractor.Plotting.Plots;

namespace AttendanceExtractor.Plotting;

public class PlotsCreator
{
    public void CreatePlots(List<AttendanceForPlots> attendances)
    {
        var statsForYears = attendances
            .GroupBy(x => x.MissionDate.Year)
            // .Select(x => x.DistinctBy(x => new {x.PlayerId, x.MissionDate.Year}))
            .Select(attendance => new StatsForYear
            {
                Year = attendance.First().MissionDate.Year,
                UniquePlayersCount = attendance.DistinctBy(x => x.PlayerId).Count(),
                MissionsCount = attendance.DistinctBy(x => x.MissionId).Count()
            })
            .ToList();

        var statsForMonths = attendances
            .GroupBy(x => new {x.MissionDate.Year, x.MissionDate.Month})
            // .Select(x => x.DistinctBy(x => x.PlayerId))
            .Select(attendance => new StatsForMonth
            {
                Year = attendance.First().MissionDate.Year,
                Month = attendance.First().MissionDate.Month,
                UniquePlayersCount = attendance.DistinctBy(x => x.PlayerId).Count(),
                MissionsCount = attendance.DistinctBy(x => x.MissionId).Count()
            })
            .ToList();

        var statsForWeekdays = attendances
            .GroupBy(x => new {x.MissionDate.DayOfWeek})
            .Select(x => new StatsForWeekday
            {
                DayOfWeek = x.First().MissionDate.DayOfWeek,
                AveragePlayersCount = (double) x.Count() / (double) x.GroupBy(x => x.MissionId).Count(),
                MissionsCount = x.DistinctBy(attendance => attendance.MissionId).Count()
            })
            .ToList();

        new StatsPerWeekdayPlot().Plot(statsForWeekdays);

        new StatsPerMonthPlot().Plot(statsForMonths);

        new StatsPerYearPlot().Plot(statsForYears);
    }
}
