namespace HandwritenRecognition.Cross.DataTransferObjects;

public class OcrLineDto
{
    public int LineId { get; set; }
    public string Text { get; set; } = string.Empty;
    public float Confidence { get; set; }
    public string Status { get; set; } = string.Empty; // ok | warning | error
    public List<List<int>> BoundingBox { get; set; } = new();
}

