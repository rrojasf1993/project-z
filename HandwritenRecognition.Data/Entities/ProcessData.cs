namespace HandwritenRecognition.Data.Entities;

public class ProcessData
{
    public Guid Id { get; set; }
    public required string Profile { get; set; }
    public float ProcessingTime { get; set; }

    public int OcrJobResultId { get; set; }
    public OcrResult Result { get; set; }
}