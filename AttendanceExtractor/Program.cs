// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using ArmaForces.R3ReplaysConverter;
using AttendanceExtractor;
using AttendanceExtractor.Plotting;
using AttendanceExtractor.Plotting.Data;
using AttendanceExtractor.Plotting.Plots;
using CsvHelper;

var shouldUpdate = true;
var cachedAttendanceString = await File.ReadAllTextAsync("attendance.json");
var attendances = JsonSerializer.Deserialize<List<Attendance>>(cachedAttendanceString, SerializationOptions.Json);

if (shouldUpdate && attendances is not null)
{
    var maximumCachedAttendance = attendances.Any()
        ? attendances.Max(attendance => attendance.CreatedAt)
        : DateTimeOffset.MinValue;

    var newAttendance = await AttendanceDownloader.GetAttendance(maximumCachedAttendance);

    var attendancesHash = new HashSet<Attendance>(attendances.Concat(newAttendance), new AttendanceEqualityComparer());

    await File.WriteAllTextAsync("attendance.json", JsonSerializer.Serialize(attendancesHash, SerializationOptions.Json));

    attendances = attendancesHash.ToList();

    Console.WriteLine("Finished");
}

var players = new Dictionary<ulong, PlayerInfo>();

var attendancesFromReplaysString = await File.ReadAllTextAsync(@"D:\Git\ArmaForces\Boderator\ArmaForces.R3ReplaysConverter\bin\Debug\net7.0\attendance-data.json");
var attendancesFromReplays = JsonSerializer.Deserialize<List<MissionAttendanceData>>(attendancesFromReplaysString)
                             ?? new List<MissionAttendanceData>();

var playerUids = attendancesFromReplays
    .SelectMany(x => x.Players)
    .GroupBy(x => x.SteamUid)
    .Select(x => new PlayerInfo
    {
        SteamUid = x.Key,
        Name = x.Select(x => x.Name)
            .GroupBy(x => x)
            .MaxBy(x => x.Count())
            .First(),
        OtherNames = x.Select(x => x.Name)
            .Distinct()
            .ToArray()
    })
    .OrderBy(x => x.Name)
    .ToList();

playerUids = playerUids
    .Concat(attendances
        .DistinctBy(x => x.PlayerId)
        .Where(attendance => playerUids.All(x => x.SteamUid != (ulong) attendance.PlayerId))
        .Select(x => new PlayerInfo
        {
            Name = string.Empty,
            SteamUid = (ulong) x.PlayerId
        }))
    .OrderBy(x => x.Name)
    .ThenBy(x => x.SteamUid)
    .ToList();

var allAttendances = attendancesFromReplays
    .Where(x => !x.MissionName.Contains("Stratis_lajf") && !x.MissionName.Contains("Altis_Lajf"))
    .Where(x => x.Players.Count > 5)
    .SelectMany(x => x.Players
        .Where(playerInfo => playerInfo.SteamUid != 0)
        .Select(playerInfo => new Attendance(Guid.NewGuid(), DateTimeOffset.Now, x.MissionId.ToString(), (long) playerInfo.SteamUid)))
    .Concat(new ExcelAttendanceExtractor().ExtractExcelAttendance())
    .Concat(attendances ?? new List<Attendance>())
    .Select(x => new AttendanceForPlots(x.Id, GetMissionDateFromId(x.MissionId), x.MissionId, x.PlayerId))
    .ToList();

new PlotsCreator().CreatePlots(allAttendances);

Console.WriteLine("Finished");

return;

DateTimeOffset GetMissionDateFromId(string missionId)
{
    var success = long.TryParse(missionId, out var epochs);
    return success
        ? DateTimeOffset.FromUnixTimeSeconds(epochs)
        : DateTimeOffset.MinValue;
}