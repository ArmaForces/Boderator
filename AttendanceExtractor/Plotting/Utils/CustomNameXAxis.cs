using ScottPlot;
using ScottPlot.AxisPanels;
using ScottPlot.TickGenerators;

namespace AttendanceExtractor.Plotting.Utils;

public class CustomNameXAxis : XAxisBase, IXAxis
{
    public override Edge Edge { get; } = Edge.Bottom;

    public CustomNameXAxis(ICollection<(double position, string label)> labels)
    {
        var positions = labels.Select(x => x.position).ToArray();
        var labelsText = labels.Select(x => x.label).ToArray();
        // TODO: Consider how to get rid of virtual member call
        TickGenerator = new NumericManual(positions, labelsText);
    }
}
