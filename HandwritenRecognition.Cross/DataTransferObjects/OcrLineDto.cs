namespace HandwritenRecognition.Cross.DataTransferObjects;

public class OcrLineDto
{
    public int LineId { get; set; }
    public string Text { get; set; } = string.Empty;
    public float ConfidenceScore { get; set; }
    public string ConfidenceStatus { get; set; } = string.Empty; // ok | warning | error
    public List<List<int>> BoundingBox { get; set; } = new();
}