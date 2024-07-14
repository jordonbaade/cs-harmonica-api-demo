using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace HarmonicaApi
{
    [ApiController]
    [Route("[controller]")]
    public class HarmonicasController(IMemoryCache memoryCache, SongDictionaryBuilder songDictionaryBuilder) : ControllerBase
    {
        [HttpGet("{key}")]
        public async ValueTask<ActionResult<List<string>>?> GetSongs(string key)
        {
            try
            {
                CheckRequest(key);
                return await GetSongsForKeyAsync(key);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500, $"Oops! There was a problem getting songs for the key of {key}");
            }
        }

        private void CheckRequest(string key)
        {
            if (IsInvalidKey(key))
            {
                throw new NoSongsFoundException($"No songs found for the key {key}");
            }
        }

        private bool IsInvalidKey(string key)
        {
            return string.IsNullOrWhiteSpace(key);
        }

        private async ValueTask<ActionResult<List<string>>?> GetSongsForKeyAsync(string key)
        {
            var songs = await GetSongsFromCacheOrDbAsync(key);
            if (songs == null || songs.Count == 0)
            {
                throw new NoSongsFoundException($"No songs found for the key {key}");
            }

            return songs;
        }

        private async ValueTask<List<string>?> GetSongsFromCacheOrDbAsync(string key)
        {
            if (memoryCache.TryGetValue(key, out List<string>? songs)) return songs;
            var songsInKeys = await songDictionaryBuilder.BuildAsync().ConfigureAwait(false);
            songsInKeys?.TryGetValue(key, out songs);
            memoryCache.Set(key, songs, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1)));

            return songs;
        }
    }
}
