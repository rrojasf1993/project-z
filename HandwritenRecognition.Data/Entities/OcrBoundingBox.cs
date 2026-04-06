namespace HandwritenRecognition.Data.Entities;

public class OcrBoundingBox
{
    public Guid Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int W { get; set; }
    public int H { get; set; }
    
    public required OcrLine SourceLine { get; set; }
}