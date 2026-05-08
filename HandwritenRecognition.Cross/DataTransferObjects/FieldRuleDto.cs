using HandwritenRecognition.Cross.Enums;

namespace HandwritenRecognition.Cross.DataTransferObjects;

public class FieldRuleDto:BaseDto
{
    public Guid Id { get; set; }

    public required FieldTypeDto FieldType { get; set; }

    public string FieldName { get; set; }

    public string? DetectionPattern { get; set; } = string.Empty;

    public string? ValidationPattern { get; set; } = string.Empty;
    public float? ConfidenceWeight { get; set; }

    public float? MinConfidence { get; set; }

    public float? Confidence { get; set; }

    public bool UseNextLine { get; set; }
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; }
    public DocumentTypeDto AssociatedDocumentType { get; set; }
    
    public RuleScopeDto Scope { get; set; }

    public RuleTypeDto Type { get; set; }
    
}