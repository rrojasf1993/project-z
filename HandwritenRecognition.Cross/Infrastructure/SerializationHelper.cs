using System.Text.Json;

namespace HandwritenRecognition.Cross.Infrastructure;

public static class SerializationHelper
{
    public static  string Serialize<T>(T item)
    {
        string? result = null;
        using (MemoryStream ms = new MemoryStream())
        {
            System.Text.Json.JsonSerializer.Serialize(ms, item,JsonSerializerOptions.Default);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            result = sr.ReadToEnd();
        }
        return result;
    }
}