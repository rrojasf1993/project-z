using System.Text.RegularExpressions;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Cross.Enums;

namespace HandwritenRecognition.Domain.Rules;

public class RegexEvaluator:IRuleEvaluator
{
    private RuleType _supportedRuleType= RuleType.Regex;

    public RuleType SupportedRuleType
    {
        get => _supportedRuleType;
        init => _supportedRuleType = value;
    }

    public ExtractedFieldDto? Evaluate(FieldRuleDto rule, OcrResultDto document)
    {
        var regexPattern=new Regex(rule.DetectionPattern);
        throw new NotImplementedException();
    }
}