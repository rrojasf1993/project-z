namespace HandwritenRecognition.Cross.DataTransferObjects;

public class OcrDocumentDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }=DateTime.Now;
    public float ConfidenceAvg { get; set; }
    public ICollection<OcrLineDto> Lines { get; set; }
}