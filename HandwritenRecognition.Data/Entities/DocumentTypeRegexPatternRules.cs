namespace HandwritenRecognition.Data.Entities;

public class DocumentTypeRegexPatternRules:BaseEntity
{
    public Guid Id { get; set; }
    public DocumentTypes DocumentType { get; set; }
    public string RegexPattern { get; set; }
    public float Weight { get; set; }
}