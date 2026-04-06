using System.Text.Json;

namespace HandwritenRecognition.Cross.Infrastructure;

public static class SerializationHelper
{
    public static  string Serialize<T>(T item)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(item);
    }
    
    public static T Deserialize<T>(string json)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    }
}