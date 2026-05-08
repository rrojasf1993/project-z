namespace HandwritenRecognition.Cross.DataTransferObjects;

public class RuleScopeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public List<FieldRuleDto> Rules { get; set; }
}