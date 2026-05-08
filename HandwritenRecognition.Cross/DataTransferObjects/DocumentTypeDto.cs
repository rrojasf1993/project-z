namespace HandwritenRecognition.Cross.DataTransferObjects;

public class DocumentTypeDto:BaseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public List<FieldRuleDto> RulesForDocType { get; set; }
    public List<FieldTypeDto> FieldTypesForDocType { get; set; }
    public List<OcrDocumentDto> Documents { get; set; }
}