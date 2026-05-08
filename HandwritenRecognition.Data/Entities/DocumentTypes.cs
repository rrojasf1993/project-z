namespace HandwritenRecognition.Data.Entities;

public class DocumentTypes:BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public List<FieldRule> RulesForDocType { get; set; }
    public List<FieldType> FieldTypesForDocType { get; set; }
    public List<OcrDocument> Documents { get; set; }
}