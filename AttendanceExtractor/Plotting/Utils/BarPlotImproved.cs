using ScottPlot;
using ScottPlot.Extensions;
using ScottPlot.Plottables;
using SkiaSharp;

namespace AttendanceExtractor.Plotting.Utils;

public class BarPlotImproved : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public string? Label { get; set; }
    public IList<BarSeries> Series { get; set; }
    public double Padding { get; set; } = 0.05;
    private double MaxBarWidth => 1 - Padding * 2;
    public Orientation Orientation { get; set; } = Orientation.Vertical;

    public LineStyle LineStyle { get; set; } = new();

    public bool GroupBarsWithSameXPosition = true; // Disable for stacked bar charts

    public BarPlotImproved(IList<BarSeries> series)
    {
        Series = series;
    }

    public IEnumerable<LegendItem> LegendItems => new []{
        new LegendItem
        {
            Label = Label,
            Children = Series.Select(barSeries => new LegendItem
            {
                Label = barSeries.Label,
                Fill = new() { Color = barSeries.Color }
            })
        }};

    public AxisLimits GetAxisLimits()
    {
        ExpandingAxisLimits limits = new();

        foreach (var s in Series)
        {
            foreach (var b in s.Bars)
            {
                if (Orientation == Orientation.Vertical)
                {
                    limits.Expand(b.Position, b.Value);
                    limits.ExpandY(b.ValueBase);
                }
                else
                {
                    limits.Expand(b.Value, b.Position);
                    limits.ExpandX(b.ValueBase);
                }
            }
        }

        limits.Left -= MaxBarWidth / 2;
        limits.Right += MaxBarWidth / 2;
        limits.Bottom -= MaxBarWidth / 2;
        limits.Top += MaxBarWidth / 2;

        return limits.AxisLimits;
    }

    public new void Render(RenderPack rp)
    {
        using var paint = new SKPaint();
        var barsByXCoordinate = Series
            .SelectMany(s => s.Bars.Select(b => (Bar: b, Series: s)))
            .ToLookup(t => t.Bar.Position);

        var maxPerXCoordinate = GroupBarsWithSameXPosition ? barsByXCoordinate.Max(g => g.Count()) : 1;
        var widthPerGroup = 1 - (maxPerXCoordinate + 1) * Padding;
        var barWidth = (1 - Padding) * widthPerGroup / (maxPerXCoordinate);

        foreach (IGrouping<double, (Bar Bar, BarSeries Series)>? group in barsByXCoordinate)
        {
            var barsInGroup = group.Count();
            var i = 0;
            foreach (var t in group)
            {
                var barWidthAndPadding = barWidth + Padding;
                var groupWidth = barWidthAndPadding * barsInGroup;

                var newPosition = GroupBarsWithSameXPosition ?
                    group.Key - groupWidth / 2 + (i + 0.5) * barWidthAndPadding :
                    group.Key;

                var rect = GetRect(t.Bar, newPosition, barWidth).ToPixelRect();
                Drawing.Fillectangle(rp.Canvas, rect, t.Series.Color);
                Drawing.DrawRectangle(rp.Canvas, rect, LineStyle.Color, LineStyle.Width);

                var text = t.Bar.Value.ToString("F");
                var fontPaint = new SKPaint(new SKFont(SKTypeface.Default, size: 16F));
                fontPaint.TextAlign = SKTextAlign.Center;
                fontPaint.IsAntialias = true;
                
                var xPx = rect.TopCenter.X;
                var yEdge = rect.TopCenter.Y - 10;
                rp.Canvas.DrawText(text, xPx, yEdge, fontPaint);

                i++;
            }
        }
    }
    
    private SKRect GetRect(Bar bar, double pos, double barWidth)
    {
        return Orientation switch
        {
            Orientation.Vertical => new SKRect(
                Axes.GetPixelX(pos - barWidth / 2),
                Axes.GetPixelY(bar.Value),
                Axes.GetPixelX(pos + barWidth / 2),
                Axes.GetPixelY(bar.ValueBase)
            ),
            Orientation.Horizontal => new SKRect(
                Axes.GetPixelX(bar.ValueBase),
                Axes.GetPixelY(pos - barWidth / 2),
                Axes.GetPixelX(bar.Value),
                Axes.GetPixelY(pos + barWidth / 2)
            ),
            _ => throw new NotImplementedException(),
        };
    }
}
