using System.Text.Json;

namespace HarmonicaApi
{
    public class CanReadWriteJson(string? filePath)
    {
        public async Task<Dictionary<string, List<string>>?> ReadJsonAsync()
        {
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, List<string>>();
            }

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
        }

        public async Task WriteJsonAsync(Dictionary<string, List<string>> songsInKeys)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(songsInKeys, options);
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
