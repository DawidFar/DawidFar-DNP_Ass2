using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileRepositories
{
    internal static class FileStorageHelper
    {
        public static async Task<List<T>> LoadListAsync<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                await File.WriteAllTextAsync(filePath, "[]");
            }

            string json = await File.ReadAllTextAsync(filePath);
            var list = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return list ?? new List<T>();
        }

        public static async Task SaveListAsync<T>(string filePath, List<T> list)
        {
            string json = JsonSerializer.Serialize(list, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
