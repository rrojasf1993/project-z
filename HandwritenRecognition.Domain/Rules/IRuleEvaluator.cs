using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Cross.Enums;

namespace HandwritenRecognition.Domain.Rules;

public interface IRuleEvaluator
{
    RuleTypeDto SupportedRuleType { get; set; }
    ExtractedFieldDto? Evaluate(FieldRuleDto rule, OcrDocumentDto? document);
}