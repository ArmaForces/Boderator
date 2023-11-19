using ScottPlot;

namespace AttendanceExtractor.Plotting.Utils;

public abstract class SoapBarPlotBase
{
    protected readonly Plot Plot;

    protected SoapBarPlotBase(string title)
    {
        Plot = new Plot();
        Plot.Title(title);
    }

    protected void SavePlot(string fileName, int width = 1920, int height = 1080)
    {
        Plot.Legend(enable: true);
        
        Plot.AutoScale();
        Plot.SetAxisLimits(bottom: 0);
        
        Plot.SaveJpeg(fileName, width, height);
    }
}
