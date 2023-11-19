// See https://aka.ms/new-console-template for more information

using ArmaForces.R3ReplaysConverter;

await ReplaysProcessing.ProcessParallel();

// using var memoryStream = new MemoryStream();

// gzipStream.CopyTo(memoryStream);

// var json = Encoding.UTF8.GetString(memoryStream.ToArray());

// var ddd = JsonSerializer.Deserialize(memoryStream, typeof(JsonObject), JsonSerializerOptions.Default);

Console.WriteLine("Hello, World!");
return;
