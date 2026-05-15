using System.Linq.Expressions;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Data.Entities;
using HandwritenRecognition.Data.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace HandwritenRecognition.Domain.Rules.DocumentClassifier;

public class HybridClassifier : IDocumentClassifier
{
    private readonly ILogger<HybridClassifier> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public HybridClassifier(ILogger<HybridClassifier> logger, IUnitOfWork unitOfWork)
    {
        this._logger = logger;
        this._unitOfWork = unitOfWork;
    }
    public Task<string> ClassifyDocumentAsync(OcrDocumentDto document)
    {

        throw new NotImplementedException();  

    }
}