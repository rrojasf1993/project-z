namespace HandwritenRecognition.Data.Entities;

public class ExtractedFields:BaseEntity
{
    public Guid Id { get; set; }
    public OcrDocument SourceOcrDocument { get; set; }
    public required string FieldName { get; set; }
    public required string Value { get; set; }
    public float Confidence { get; set; }
    public FieldRule? Rule { get; set; }
    public bool WasHumanCorrected { get; set; }
}