using System.Text.Json.Serialization;

namespace HandwritenRecognition.Cross.DataTransferObjects;

public class InputFileDto
{
    [JsonPropertyName("path")]
    public required string Path { get; set; }
}