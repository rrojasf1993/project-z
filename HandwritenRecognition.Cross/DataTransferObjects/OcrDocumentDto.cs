namespace HandwritenRecognition.Cross.DataTransferObjects;

public class OcrDocumentDto:BaseDto
{
    public Guid Id { get; set; }
    public float ConfidenceAvg { get; set; }
    public List<OcrLineDto> Lines { get; set; }

    public OcrDocumentStatus_ Status { get; set; }
}