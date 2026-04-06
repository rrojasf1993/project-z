namespace HandwritenRecognition.Cross.DataTransferObjects;

public class OcrDocumentDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }=DateTime.Now;
    public float ConfidenceAvg { get; set; }
    public List<OcrLineDto> Lines { get; set; }
    public DateTime UpdatedAt { get; set; }=DateTime.Now;
}