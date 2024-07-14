namespace HarmonicaApi;

public class SongDictionaryBuilder(CanReadWriteJson json)
{
    public async Task<Dictionary<string, List<string>>?> BuildAsync()
    {
        return await json.ReadJsonAsync();
    }
}