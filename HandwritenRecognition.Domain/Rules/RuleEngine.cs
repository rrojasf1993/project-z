using HandwritenRecognition.Cross.DataTransferObjects;

namespace HandwritenRecognition.Domain.Rules;

public class RuleEngine(IEnumerable<IRuleEvaluator> ruleEvaluators)
{
    public List<ExtractedFieldDto> Run(List<FieldRuleDto> fieldRules,OcrResultDto ocrDocument)
    {
        List<ExtractedFieldDto> extractedFields = new List<ExtractedFieldDto>();
        foreach (var rule in fieldRules)
        {
            var ruleEvaluatorInstance=ruleEvaluators.FirstOrDefault(ev=>ev.SupportedRuleType==rule.Type);
            if(ruleEvaluatorInstance is null)
                continue;
            var evaluationResult=ruleEvaluatorInstance.Evaluate(rule, ocrDocument);
         if(evaluationResult is not null)
                extractedFields.Add(evaluationResult);
        }
        return extractedFields;
        
    }
}

