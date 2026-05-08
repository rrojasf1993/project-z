using System.Linq.Expressions;
using AutoMapper;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Data.Entities;
using HandwritenRecognition.Data.UnitOfWork;
using HandwritenRecognition.Domain.Rules;
using Microsoft.Extensions.Logging;

namespace HandwritenRecognition.Services;

public class RuleService(IUnitOfWork unitOfWork, ILogger<RuleService> logger, IMapper mapper)
    : IRuleService
{
    public IEnumerable<FieldRuleDto>? GetRules(DocumentTypeDto? documentTypeDto)
    {
        try
        {
            Expression<Func<FieldRule, bool>> filterExpression;
            if(documentTypeDto != null)
                filterExpression = o => o.AssociatedDocumentType.Id == documentTypeDto.Id && o.IsActive == true;
            filterExpression=frEx=>frEx.IsActive==true;
            var currentRules=unitOfWork.FieldRules.Get(
                filter: filterExpression,
                includeProperties: "Scope,Type");
            return mapper.Map<IEnumerable<FieldRuleDto>>(currentRules);
        }
        catch (Exception e)
        {
            logger.LogError("An error has occurred getting the rules for feature extraction\n {0}\n {1}", e.Message, e.StackTrace);
            return null;
        }
    }

    public List<ExtractedFieldDto>? ExtractFieldsForDocument(OcrDocumentDto correctedDocument, IEnumerable<FieldRuleDto?> applicableRules)
    {
        List<IRuleEvaluator> evaluators = GetRuleEvaluators();
        return null;
    }

    private List<IRuleEvaluator> GetRuleEvaluators()
    {
        throw new NotImplementedException();
    }
}