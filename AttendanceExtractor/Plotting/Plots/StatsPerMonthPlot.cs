using AttendanceExtractor.Plotting.Data;
using AttendanceExtractor.Plotting.Utils;
using ScottPlot;
using ScottPlot.Plottables;

namespace AttendanceExtractor.Plotting.Plots;

public class StatsPerMonthPlot : SoapBarPlotBase
{
    private const string Title = "Liczba graczy w miesiącu";
    private const string FileName = "stats-per-month.jpg";
    
    private const int FirstYear = 2017;
    
    public StatsPerMonthPlot() : base(title: Title) { }
    
    public void Plot(ICollection<StatsForMonth> statsForMonths)
    {
        var barsUniquePlayersPerMonth = statsForMonths
            .Select(x => new Bar(position: GetPositionForMonth(x.Year, x.Month), value: x.UniquePlayersCount))
            .ToList();
        var barMissionsPerMonth = statsForMonths
            .Select(x => new Bar(position: GetPositionForMonth(x.Year, x.Month), value: x.MissionsCount))
            .ToList();

        var statsPerMonthPlot = new BarPlotImproved(new List<BarSeries>
        {
            new()
            {
                Bars = barsUniquePlayersPerMonth,
                Color = ExcelColors.LightBlue,
                Label = "Liczba osób w miesiącu"
            },
            new()
            {
                Bars = barMissionsPerMonth,
                Color = ExcelColors.Red,
                Label = "Suma gier"
            }
        });
        
        var positions = statsForMonths
            .Select(x => ((double) GetPositionForMonth(x.Year, x.Month), $"{x.Year}{Environment.NewLine}{Names.GetPolishMonthName(x.Month)}"))
            .ToArray();
        
        base.Plot.Add.Plottable(statsPerMonthPlot);
        base.Plot.AxisStyler.ClearAxes(Edge.Bottom);
        base.Plot.XAxes.Add(new CustomNameXAxis(positions));
        
        base.Plot.RightAxis.FrameLineStyle.Color = Colors.White;
        
        SavePlot(FileName);
    }

    private static int GetPositionForMonth(int year, int month)
        => (year - FirstYear) * 12 + month;
}

public class StatsPerYearPlot : SoapBarPlotBase
{
    private const string Title = "Liczba graczy w roku";
    private const string FileName = "stats-per-year.jpg";
    
    private const int FirstYear = 2017;
    
    public StatsPerYearPlot() : base(title: Title) { }
    
    public void Plot(ICollection<StatsForYear> statsForYears)
    {
        var barsUniquePlayersPerMonth = statsForYears
            .Select(x => new Bar(position: GetPositionForMonth(x.Year), value: x.UniquePlayersCount))
            .ToList();
        var barMissionsPerMonth = statsForYears
            .Select(x => new Bar(position: GetPositionForMonth(x.Year), value: x.MissionsCount))
            .ToList();

        var statsPerMonthPlot = new BarPlotImproved(new List<BarSeries>
        {
            new()
            {
                Bars = barsUniquePlayersPerMonth,
                Color = ExcelColors.LightBlue,
                Label = "Liczba osób w roku"
            },
            new()
            {
                Bars = barMissionsPerMonth,
                Color = ExcelColors.Red,
                Label = "Suma gier"
            }
        });
        
        var positions = statsForYears
            .Select(x => ((double) GetPositionForMonth(x.Year), x.Year.ToString()))
            .ToArray();
        
        base.Plot.Add.Plottable(statsPerMonthPlot);
        base.Plot.AxisStyler.ClearAxes(Edge.Bottom);
        base.Plot.XAxes.Add(new CustomNameXAxis(positions));
        
        base.Plot.RightAxis.FrameLineStyle.Color = Colors.White;
        
        SavePlot(FileName);
    }

    private static int GetPositionForMonth(int year) => year - FirstYear;
}