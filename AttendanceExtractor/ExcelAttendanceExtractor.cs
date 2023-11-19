using System.Globalization;
using CsvHelper;

namespace AttendanceExtractor;

public class ExcelAttendanceExtractor
{
    private const string ExcelAttendancePath = @"D:\excel-attendance.csv";
    
    public List<Attendance> ExtractExcelAttendance()
    {
        using var stream = new StreamReader(ExcelAttendancePath);
        using var csvReader = new CsvReader(stream, CultureInfo.CurrentCulture);

        csvReader.Read();
        csvReader.ReadHeader();
        var missionDates = csvReader.HeaderRecord
            .Where(x => x != "User" && x != "SteamId")
            .Select(x => DateTimeOffset.ParseExact(x, "dd.MM.yyyy", CultureInfo.CurrentCulture))
            .Select(x => (x, new List<long>()))
            .ToList();

        var rowIndex = 0;
        while (csvReader.Read())
        {
            rowIndex++;
            var hasUid = long.TryParse(csvReader.GetField(0), out var playerUid);
            var playerName = csvReader.GetField(1);
            
            for (var i = 2; i < missionDates.Count; i++)
            {
                var wasPlayerOnMission = csvReader.GetField<double?>(i);
                if (wasPlayerOnMission.HasValue && Math.Abs(wasPlayerOnMission.Value - 1) < 0.001)
                {
                    missionDates[i-2].Item2.Add(hasUid ? playerUid : rowIndex);
                }
            }
        }

        return missionDates
            .SelectMany(tuple => tuple.Item2
                .Select(x => new Attendance(Guid.NewGuid(), DateTimeOffset.Now, tuple.x.ToUnixTimeSeconds().ToString(), x)))
            .ToList();
    }
}
