using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Cross.Enums;

namespace HandwritenRecognition.Domain.Rules;

public interface IRuleEvaluator
{
    RuleType SupportedRuleType { get; init; }
    ExtractedFieldDto? Evaluate(FieldRuleDto rule, OcrResultDto document);
}