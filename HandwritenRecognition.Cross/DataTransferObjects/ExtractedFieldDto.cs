namespace HandwritenRecognition.Cross.DataTransferObjects;

public class ExtractedFieldDto:BaseDto
{
    public Guid Id { get; set; }
    public Guid OcrDocumentId { get; set; }
    public string FieldName { get; set; }
    public string Value { get; set; }
    public float Confidence { get; set; }
    public Guid RuleId { get; set; }
    public bool WasHumanCorrected { get; set; }
}