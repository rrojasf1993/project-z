using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;

namespace HandwritenRecognition.Data.Entities;

public class OcrJob:BaseEntity
{
    public Guid JobId { get; set; }
    public OcrJobStatus Status { get; set; }
    public OcrResult? Result { get; set; }
    public string? Error { get; set; }
    public required string FileName { get; set; }
    public string? PreprocessedFileName { get; set; }

}