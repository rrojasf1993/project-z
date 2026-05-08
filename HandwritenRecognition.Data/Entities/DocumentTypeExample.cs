namespace HandwritenRecognition.Data.Entities;

public class DocumentTypeExample
{
    public Guid Id { get; set; }
    public DocumentTypes Type { get; set; }
    public string SampleText { get; set; }
    public string EmbeddingVector { get; set; }
}