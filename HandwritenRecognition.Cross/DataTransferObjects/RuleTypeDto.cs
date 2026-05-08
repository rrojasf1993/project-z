namespace HandwritenRecognition.Cross.DataTransferObjects;

public class RuleTypeDto
{
    public Guid Id { get; set; }
    public string Kind { get; set; }
    public string Notes { get; set; }
    public List<FieldRuleDto> Rules { get; set; }
}