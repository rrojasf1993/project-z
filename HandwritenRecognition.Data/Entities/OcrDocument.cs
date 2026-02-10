namespace HandwritenRecognition.Data.Entities;

public class OcrDocument
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public float ConfidenceAvg { get; set; }
    public string OriginalFileName { get; set; }
    public ICollection<OcrLine> Lines { get; set; }
    public string RawOcrResult { get; set; }
    
}