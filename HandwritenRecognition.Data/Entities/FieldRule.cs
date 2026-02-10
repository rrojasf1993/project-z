namespace HandwritenRecognition.Data.Entities;

public class FieldRule
{
    public Guid Id { get; set; }
    public required string DocumentType { get; set; } = "";
    public required string RegexPattern { get; set; }=string.Empty;
    public float Weight { get; set; }
    public bool IsActive { get; set; }
    public int Priority { get; set; }
    
}