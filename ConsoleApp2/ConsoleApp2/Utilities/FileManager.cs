using System.Text.Json;

namespace SteamApp.Utilities
{
    public static class FileManager
    {
        public static void Save<T>(string path, T data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, JsonSerializer.Serialize(data));
        }

        public static T? Load<T>(string path)
        {
            if (!File.Exists(path)) return default;
            return JsonSerializer.Deserialize<T>(File.ReadAllText(path));
        }
    }
}