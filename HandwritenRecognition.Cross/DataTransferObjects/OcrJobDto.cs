namespace HandwritenRecognition.Cross.DataTransferObjects;

public class OcrJobDto
{
    public Guid JobId { get; set; }
    public OcrJobStatus Status { get; set; }
    public OcrResultDto? Result { get; set; }
    public string? Error { get; set; }
}