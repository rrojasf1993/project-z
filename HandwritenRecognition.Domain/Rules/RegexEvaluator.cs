using System.Text.RegularExpressions;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Cross.Enums;

namespace HandwritenRecognition.Domain.Rules;

public class RegexEvaluator : IRuleEvaluator
{
    private RuleTypeDto _supportedRuleType;

    public RuleTypeDto SupportedRuleType
    {
        get => _supportedRuleType;
        set => _supportedRuleType = value;
    }

    private string GetTextToEvaluate(OcrLineDto line)
    {
        if (!string.IsNullOrWhiteSpace(line.CorrectedText) && line.CorrectedText != line.OriginalText)
            return line.CorrectedText;
        return line.OriginalText;
    }

    public ExtractedFieldDto? Evaluate(FieldRuleDto rule, OcrDocumentDto? document)
    {
        if (string.IsNullOrWhiteSpace(rule.DetectionPattern))
            return null;
        Regex rgx = new Regex(rule.DetectionPattern, RegexOptions.IgnoreCase);
        if (document?.Lines == null) return null;
        foreach (var line in document?.Lines)
        {
            var text = GetTextToEvaluate(line);
            Match match = rgx.Match(text);
            if (!match.Success)
                continue;
            var value = match.Value;
            if (rule.UseNextLine)
            {
                OcrLineDto? nextLine = document.Lines.FirstOrDefault(l => l.LineIndex == line.LineIndex + 1);
                if (nextLine != null)
                {
                    text = GetTextToEvaluate(nextLine);
                    match = rgx.Match(text);
                }
            }

            if (!string.IsNullOrWhiteSpace(rule.ValidationPattern))
            {
                Regex validationsRegex = new Regex(rule.ValidationPattern, RegexOptions.IgnoreCase);
                if (!validationsRegex.IsMatch(text))
                    continue;
            }

            if (rule.MinConfidence.HasValue && rule.Confidence < rule.MinConfidence)
                continue;
            return new ExtractedFieldDto()
            {
                FieldName = rule.FieldName,
                Value = value,
                Confidence = line.ConfidenceScore * (rule.ConfidenceWeight ?? 1)
            };
        }

        return null;
    }

}