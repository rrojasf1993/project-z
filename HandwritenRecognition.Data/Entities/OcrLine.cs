using System;

namespace HandwritenRecognition.Data.Entities;

public class OcrLine
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public OcrDocument Document { get; set; }
    public int LineIndex { get; set; }
    public string OriginalText { get; set; } = string.Empty;
    public string? CorrectedText { get; set; }
    public float Confidence { get; set; }
    public string Status { get; set; } = string.Empty;
    public string BoundingBox { get; set; } 
}
