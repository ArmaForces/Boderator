using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace ArmaForces.R3ReplaysConverter;

public static class ReplaysProcessing
{
    private const string ReplaysFolder = "D:\\ArmaForcesR3Replays";
    private static readonly string DataPath = Path.Join(Directory.GetCurrentDirectory(), "attendance-data.json");
    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions();

    private static readonly string[] MissionsWithBrokenUnitsJsons =
    {
        "208#Mydlana_Misja_29_09#sara#26#29-09-2018 20-33-30#",
        "2791#"
    };
    
    private static readonly object DataLock = new object();
    
    public static async Task ProcessParallel()
    {
        var data = await GetExistingData();
        var replays = GetReplaysToProcess(data);
        
        await Parallel.ForEachAsync(replays, (replay, token) => ProcessReplay(replay, data));
        
        SaveData(data);
    }

    public static async Task ProcessSequential()
    {
        var data = await GetExistingData();
        var replays = GetReplaysToProcess(data);
        
        foreach (var replay in replays)
        {
            await ProcessReplay(replay, data);
        }
        
        SaveData(data);
    }

    private static IEnumerable<string> GetReplaysToProcess(List<MissionAttendanceData> ddd)
    {
        var processedReplays = ddd.Select(x => x.ReplayFilePath).ToList();
        
        Console.WriteLine($"Found {processedReplays.Count} already processed replays");
        var remainingReplays = Directory.GetFiles(ReplaysFolder)
            .Where(x => !processedReplays.Contains(x))
            // .Where(replay => !MissionsWithBrokenUnitsJsons.Any(replay.Contains))
            // 208 requires special handling for "" in group name
            // .Where(x => !x.Contains("208#Mydlana_Misja_29_09#sara#26#29-09-2018 20-33-30#.json"))
            .ToList();
        
        Console.WriteLine($"{remainingReplays.Count} replays remaining");

        return remainingReplays; //.First(x => x.Contains("77#"));
    }

    private static async Task<List<MissionAttendanceData>> GetExistingData()
    {
        return File.Exists(DataPath)
            ? JsonSerializer.Deserialize<List<MissionAttendanceData>>(await File.ReadAllTextAsync(DataPath), SerializerOptions)
              ?? new List<MissionAttendanceData>()
            : new List<MissionAttendanceData>();
    }

    private static void SaveData(List<MissionAttendanceData> data)
    {
        Console.WriteLine($"Saving data for {data.Count} replays.");
        File.WriteAllText(DataPath, JsonSerializer.Serialize(data, SerializerOptions), Encoding.UTF8);
    }
    
    private static async ValueTask ProcessReplay(string replay, List<MissionAttendanceData> data)
    {
        var replayFileName = Path.GetFileName(replay).Replace("#.json.gz", string.Empty);
        var fileNameComponents = replayFileName.Split('#');
        var replayIndex = fileNameComponents[0];
        var missionName = fileNameComponents[1];
        var mapCode = fileNameComponents[2];
        var missionNumberThatDay = fileNameComponents[3];
        var missionDateText = fileNameComponents[4];
        var success = DateTimeOffset.TryParseExact(missionDateText, "dd-MM-yyyy HH-mm-ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var missionDate);
        if (!success)
        {
        
        }
    
        Console.WriteLine($"Processing {replayFileName}");
    
        await using var fileStream = new FileStream(replay, FileMode.Open);
        await using var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
        var players = await Utf8ReaderPartialRead.Run(gzipStream);

        var attendanceData = new MissionAttendanceData
        {
            MapCode = mapCode,
            MissionDate = missionDate,
            MissionName = missionName,
            Players = players,
            ReplayFilePath = replay
        };

        lock (DataLock)
        {
            data.Add(attendanceData);

            if (data.Count % 50 == 0)
            {
                SaveData(data);
            }
        }
    }
}
