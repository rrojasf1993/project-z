namespace HandwritenRecognition.Cross.DataTransferObjects;

public class OcrJobEvent:BaseEvent<OcrResultDto>
{
     public Guid JobId { get; set; }
     public string? ErrorDetails { get; set; }
}