namespace RetroMikeMiningTools.Utilities
{
    public static class WebUtilities
    {
        public static async Task DownloadFile(string url, string pathToSave, string fileName)
        {
            var content = await GetUrlContent(url);
            if (content != null)
            {
                if (!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);
                await File.WriteAllBytesAsync($"{pathToSave}/{fileName}", content);
            }
        }

        public static async Task<byte[]?> GetUrlContent(string url)
        {
            using (var client = new HttpClient())
            using (var result = await client.GetAsync(url))
            {
                return result.IsSuccessStatusCode ? await result.Content.ReadAsByteArrayAsync() : null;
            }
        }
    }
}
