namespace HandwritenRecognition.Data.Entities;

public class RuleType
{
    public Guid Id { get; set; }
    public string Kind { get; set; }
    public string Notes { get; set; }
    public List<FieldRule> Rules { get; set; }
}