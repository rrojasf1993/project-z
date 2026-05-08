namespace HandwritenRecognition.Data.Entities;

public class DocumentTypeKeywordRules:BaseEntity
{
    public Guid Id { get; set; }
    public required DocumentTypes Type { get; set; }
    public required string Keyword { get; set; }
    public required float Weight { get; set; }
}