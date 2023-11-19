using AttendanceExtractor.Plotting.Data;
using AttendanceExtractor.Plotting.Utils;
using ScottPlot;
using ScottPlot.Plottables;

namespace AttendanceExtractor.Plotting.Plots;

public class StatsPerWeekdayPlot : SoapBarPlotBase
{
    private const string Title = "Średnia frekwencja wg. dnia tygodnia";
    private const string FileName = "stats-per-weekday.jpg";

    public StatsPerWeekdayPlot() : base(title: Title) { }
    
    public void Plot(ICollection<StatsForWeekday> statsForWeekdays)
    {
        var barsPlayersPerWeekday = statsForWeekdays
            .Select(x => new Bar(position: GetDayOfWeekPosition(x.DayOfWeek), value: x.AveragePlayersCount))
            .ToList();
        var barMissionsPerWeekday = statsForWeekdays
            .Select(x => new Bar(position: GetDayOfWeekPosition(x.DayOfWeek), value: x.MissionsCount))
            .ToList();

        var statsPerWeekdayPlot = new BarPlotImproved(new List<BarSeries>
        {
            new()
            {
                Bars = barsPlayersPerWeekday,
                Color = ExcelColors.LightBlue,
                Label = "Średnia liczba osób"
            },
            new()
            {
                Bars = barMissionsPerWeekday,
                Color = ExcelColors.Red,
                Label = "Suma gier"
            }
        });

        var positions = statsForWeekdays
            .Select(x => ((double)GetDayOfWeekPosition(x.DayOfWeek), Names.GetPolishWeekdayName(x.DayOfWeek)))
            .ToArray();

        base.Plot.Add.Plottable(statsPerWeekdayPlot);
        base.Plot.AxisStyler.ClearAxes(Edge.Bottom);
        base.Plot.XAxes.Add(new CustomNameXAxis(positions));

        base.Plot.RightAxis.FrameLineStyle.Color = Colors.White;
        
        SavePlot(FileName);
    }

    private static int GetDayOfWeekPosition(DayOfWeek dayOfWeek) =>
        dayOfWeek == DayOfWeek.Sunday
            ? (int) DayOfWeek.Saturday + 1
            : (int) dayOfWeek;
}