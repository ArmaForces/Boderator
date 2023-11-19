using System.Text.Json;

namespace AttendanceExtractor;

public static class AttendanceDownloader
{
    public static async Task<List<Attendance>> GetAttendance(DateTimeOffset createdAfter)
    {
        var httpClient = new HttpClient();

        var i = 1;
        var lastPageReached = false;
        var attendances = new List<Attendance>();
        do
        {
            var requestUrl =
                $"https://armaforces.com/api/attendances?limit=50&order[createdAt]=asc&createdAt[after]={createdAfter:yyyy-MM-ddTHH:mm:ssK}&page={i}";
            var response = await httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var attendanceResponse = JsonSerializer.Deserialize<AttendanceResponse>(responseString, SerializationOptions.Json);

            if (attendanceResponse is null)
            {
                throw new Exception($"Failed deserialization of {responseString}");
            }

            if (attendanceResponse.CurrentPage == attendanceResponse.LastPage)
            {
                lastPageReached = true;
            }
            else
            {
                i++;
            }
    
            attendances.AddRange(attendanceResponse.Data);
        } while (!lastPageReached);

        return attendances;
    }
}
