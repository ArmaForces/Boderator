using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ArmaForces.R3ReplaysConverter;

public class Utf8ReaderPartialRead
{
    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static List<string> KnownUselessEventTypes = new List<string>
    {
        "player_connected",
        "player_disconnected",
        "unit_killed",
        "get_in",
        "get_out",
        "positions_vehicles"
    };

    public static List<PlayerInfo> RunDebug(Stream stream)
    {
        var buffer = new byte[4096*2];
        _ = stream.Read(buffer);
        var debugReader = new Utf8JsonReader(buffer, isFinalBlock: false, state: default);
        Console.WriteLine($"String in buffer is: {Encoding.UTF8.GetString(buffer)}");
        // GetMoreBytesFromStream(stream, ref buffer, ref debugReader);
        if (debugReader.TokenType == JsonTokenType.StartObject)
        {
            JsonSerializer.Deserialize<R3Entry>(debugReader.Read());
        }

        return new List<PlayerInfo>();
    }
    
    public static async Task<List<PlayerInfo>> Run(Stream stream)
    {
        var items = JsonSerializer.DeserializeAsyncEnumerable<R3Entry>(stream, SerializerOptions);

        var playerInfos = new Dictionary<string, PlayerInfo>();

        await foreach (var item in items)
        {
            if (item is null || item.Type == "markers") continue;

            if (item.Type == "positions_infantry")
            {
                List<R3UnitEntry> units;
                try
                {
                    units = JsonSerializer.Deserialize<List<R3UnitEntry>>(item.Value, SerializerOptions) ?? new List<R3UnitEntry>();
                }
                catch (Exception)
                {
                    continue;
                }
                
                var players = units?.Where(x => x.Id != "0").ToArray();
                foreach (var player in players ?? Array.Empty<R3UnitEntry>())
                {
                    var success = playerInfos.TryGetValue(player.Id, out _);
                    if (success)
                    {
                        continue;
                    }
                    else
                    {
                        var nameMatch = Regex.Match(player.Unit, @"\(.*\)");
                        var playerName = nameMatch.Success
                            ? nameMatch.Value.Trim('(').Trim(')')
                            : player.Unit;
                        var uidParseSuccess = ulong.TryParse(player.Id, out var playerUid);

                        if (!uidParseSuccess) continue;
                        
                        playerInfos.Add(player.Id, new PlayerInfo
                        {
                            Name = playerName,
                            SteamUid = uidParseSuccess ? playerUid : 0
                        });
                    }
                }
            }
            else
            {
                if (!KnownUselessEventTypes.Contains(item.Type))
                {
                    KnownUselessEventTypes.Add(item.Type);
                }
            }
        }
        
        // var buffer = new byte[4096];
        // _ = stream.Read(buffer);
        // var reader = new Utf8JsonReader(buffer, isFinalBlock: false, state: default);
        // Console.WriteLine($"String in buffer is: {Encoding.UTF8.GetString(buffer)}");
        // GetMoreBytesFromStream(stream, ref buffer, ref reader);
        // if (reader.TokenType == JsonTokenType.StartObject)
        // {
        //     JsonSerializer.Deserialize<R3Entry>(reader.);
        // }

        return playerInfos.Values.ToList();
    }
    
    private static void GetMoreBytesFromStream(
        Stream stream, ref byte[] buffer, ref Utf8JsonReader reader)
    {
        int bytesRead;
        if (reader.BytesConsumed < buffer.Length)
        {
            ReadOnlySpan<byte> leftover = buffer.AsSpan((int)reader.BytesConsumed);

            if (leftover.Length == buffer.Length)
            {
                Array.Resize(ref buffer, buffer.Length * 2);
                Console.WriteLine($"Increased buffer size to {buffer.Length}");
            }

            leftover.CopyTo(buffer);
            bytesRead = stream.Read(buffer.AsSpan(leftover.Length));
        }
        else
        {
            bytesRead = stream.Read(buffer);
        }
        Console.WriteLine($"String in buffer is: {Encoding.UTF8.GetString(buffer)}");
        reader = new Utf8JsonReader(buffer, isFinalBlock: bytesRead == 0, reader.CurrentState);
    }
}
