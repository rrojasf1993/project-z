using AutoMapper;
using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Data.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HandwritenRecognition.Services;

public class DocumentService(IMapper mapperInstance, ILogger<DocumentService> logger, IUnitOfWork unitOfWork)
    : IDocumentService
{
    private readonly IMapper _mapperInstance = mapperInstance;
    private readonly ILogger<DocumentService> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public List<OcrDocumentDto> GetOcrDocumentsByStatus(OcrDocumentStatus_ status)
    {
        var query=_unitOfWork.OcrDocuments.Get(
            filter: x => x.Status == status);
        List<OcrDocumentDto> documents = _mapperInstance.Map<List<OcrDocumentDto>>(query).ToList();
        return documents;
    }
}