using HandwritenRecognition.Cross.DataTransferObjects;

namespace HandwritenRecognition.Services;

public interface IRuleService
{
    IEnumerable<FieldRuleDto>? GetRules(DocumentTypeDto? documentTypeDto);
}