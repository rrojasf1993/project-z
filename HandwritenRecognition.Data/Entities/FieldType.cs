namespace HandwritenRecognition.Data.Entities;

public class FieldType:BaseEntity
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string? Notes { get; set; }
    public List<FieldRule> Rules { get; set; }
    public DocumentTypes? SrcDocumentType { get; set; }
}