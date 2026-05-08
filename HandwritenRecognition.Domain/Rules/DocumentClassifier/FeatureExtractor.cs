using System.Linq.Expressions;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Data.Entities;
using HandwritenRecognition.Data.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HandwritenRecognition.Domain.Rules.DocumentClassifier;

public class FeatureExtractor
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    public FeatureExtractor(ILogger loggerInstance, IUnitOfWork unitOfWorkInstance)
    {
        this._logger = loggerInstance;
        this._unitOfWork = unitOfWorkInstance;
    }
    
    public OcrFeatureSet Extract(OcrDocumentDto document)
    {
        if (document.Lines is null || document.Lines.Count == 0)
            return null;
        var lineIds = document.Lines.Select(l => l.LineId);
        Expression<Func<OcrBoundingBox, bool>> boundingBoxExpr = b => lineIds.Contains(b.SourceLine.Id);
        var orderedBouningBoxes=_unitOfWork.BoundingBoxes.Get(filter: boundingBoxExpr, includeProperties:"SourceLine").OrderBy(bBox => bBox.Y);
        OcrFeatureSet featuresForDocument = new OcrFeatureSet()
        {

        };
        return null;
    }
}