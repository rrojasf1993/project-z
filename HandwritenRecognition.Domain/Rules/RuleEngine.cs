using HandwritenRecognition.Cross.DataTransferObjects;

namespace HandwritenRecognition.Domain.Rules;

public class RuleEngine(IEnumerable<IRuleEvaluator> ruleEvaluators)
{
    public List<ExtractedFieldDto?> Run(List<FieldRuleDto> fieldRules, OcrDocumentDto ocrDocument)
    {
        var query = from rule in fieldRules.OrderBy(r => r.Priority)
            let ruleEvaluatorInstance =
                ruleEvaluators.FirstOrDefault(rev => rev.SupportedRuleType.Id.Equals(rule.Id))
            select new
            {
                currentRuleEvaluator = ruleEvaluatorInstance,
                currentFieldRule = rule
            };
        return query.Select(fieldRuleEvaluator =>
                fieldRuleEvaluator.currentRuleEvaluator.Evaluate(fieldRuleEvaluator.currentFieldRule, ocrDocument))
            .ToList();
    }
}