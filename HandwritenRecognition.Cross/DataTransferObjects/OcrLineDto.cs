using System.Runtime.CompilerServices;

namespace HandwritenRecognition.Cross.DataTransferObjects;

public class OcrLineDto
{
    public Guid LineId { get; set; }
    public int LineIndex { get; set; }
    public string OriginalText { get; set; } = string.Empty;
    public string? CorrectedText { get; set; } = string.Empty;
    public float ConfidenceScore { get; set; }
    public string ConfidenceStatus { get; set; } = string.Empty; // ok | warning | error
    public List<List<int>> BoundingBox { get; set; } 
    public List<OcrBoundingBoxDto> FrontendFriendlyBoxes {get; set;}
}
