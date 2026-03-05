namespace HandwritenRecognition.Data.Entities;

public class ExtractedFields
{
    public Guid Id { get; set; }
    public Guid OcrDocumentId { get; set; }
    public required string FieldName { get; set; }
    public required string Value { get; set; }
    public float Confidence { get; set; }
    public Guid RuleId { get; set; }
    public bool WasHumanCorrected { get; set; }
    public DateTime CreatedAt { get; set; }
}