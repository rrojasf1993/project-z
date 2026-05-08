namespace HandwritenRecognition.Cross.DataTransferObjects;

public class FieldTypeDto:BaseDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string? Notes { get; set; }
    public List<FieldRuleDto> Rules { get; set; }
    public DocumentTypeDto? SrcDocumentType { get; set; }
}