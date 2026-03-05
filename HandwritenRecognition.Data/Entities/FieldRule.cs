using System;
using HandwritenRecognition.Cross.Enums;

namespace HandwritenRecognition.Data.Entities;

public class FieldRule
{
    public Guid Id { get; set; }
    public string DocumentType { get; set; }
    public FieldType? FieldType { get; set; }

    public string FieldName { get; set; }

    public string? DetectionPattern { get; set; } = string.Empty;
    
    public string? ValidationPattern { get; set; } = string.Empty;
    public float? ConfidenceWeight { get; set; }

    public float? MinConfidence { get; set; }

    public bool UseNextLine { get; set; }
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; }
    public RuleScope Scope { get; set; }
    
    public RuleType Type { get; set; }
}