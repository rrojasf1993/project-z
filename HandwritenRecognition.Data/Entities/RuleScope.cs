using System.Data;

namespace HandwritenRecognition.Data.Entities;

public class RuleScope
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
    public List<FieldRule> Rules { get; set; }
}