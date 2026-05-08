using System.Linq.Expressions;
using AutoMapper;
using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Data.Entities;
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

    public List<OcrDocumentDto>? GetOcrDocumentsByStatus(OcrDocumentStatus_ status, DateTime? startDate,
        DateTime? endDate)
    {
        _logger.LogInformation($"Getting documents with {status} ...");
        Expression<Func<OcrDocument, bool>> expression = o => o.Status == status;
        if (startDate.HasValue && endDate.HasValue)
        {
            expression = o => o.Status == status && o.UpdatedAt >= startDate.Value && o.UpdatedAt <= endDate.Value;
            _logger.LogInformation(
                $"Getting documents with {status} , start date {startDate.GetValueOrDefault().ToShortDateString()} , end date {endDate.GetValueOrDefault().ToShortDateString()} ");
        }

        List<OcrDocumentDto>? documents = new List<OcrDocumentDto>();
        try
        {
            var query = _unitOfWork.OcrDocuments.Get(
                filter: expression, includeProperties: "Lines");
            foreach (var document in query)
            {
                var linesIds = document.Lines.Select(l => l.Id).ToList();
                Expression<Func<OcrBoundingBox, bool>> boundingBoxExpr = b => linesIds.Contains(b.SourceLine.Id);
                var boundingBoxes =
                    _unitOfWork.BoundingBoxes.Get(filter: boundingBoxExpr, includeProperties: "SourceLine");
                var documentInstance = _mapperInstance.Map<OcrDocumentDto>(document);
                documentInstance.Lines.ForEach(line =>
                {
                    line.FrontendFriendlyBoxes =
                        _mapperInstance.Map<List<OcrBoundingBoxDto>>(
                            boundingBoxes.Where(b => b.SourceLine.Id == line.LineId));
                });
                documents.Add(documentInstance);
            }

            documents = _mapperInstance.Map<List<OcrDocumentDto>>(query);
        }
        catch (Exception e)
        {
            _logger.LogError(
                $"An error has ocurred querying the documents with state {status} ...\\n{e.Message}\\{e.StackTrace}");
        }

        return documents;
    }

    public async Task<OcrDocumentDto?> UpdateOcrDocument_WithHumanCorrections(OcrDocumentDto editedOcrDocument)
    {
        _logger.LogInformation(
            $"Updating ocr document with id {editedOcrDocument.Id} , checking if there is any human correction to be applied");
        var updatedLinesInDocument = editedOcrDocument
            .Lines
            .Where(l =>
                !string.IsNullOrWhiteSpace(l.CorrectedText) && l.CorrectedText != l.OriginalText);
        OcrDocument? editDocumentInstance = null;
        if (updatedLinesInDocument.Any())
        {
            logger.LogInformation(
                $"There were {updatedLinesInDocument.Count()} human modified lines in the ocr document");
            var lineIdsToUpdate = updatedLinesInDocument.Select(ul => ul.LineId);
            Expression<Func<OcrLine, bool>> linesToUpdateFilter = o => lineIdsToUpdate.Contains(o.Id);
            var updatableLines = _unitOfWork.Lines.Get(filter: linesToUpdateFilter);
            updatableLines.AsParallel().ForAll((line) =>
            {
                var correctedLine =
                    editedOcrDocument.Lines.FirstOrDefault(editedDocumentLine => editedDocumentLine.LineId == line.Id);
                if (correctedLine is not null)
                {
                    line.CorrectedText = correctedLine.CorrectedText;
                    _unitOfWork.Lines.Update(line);
                }
            });
            _logger.LogInformation("Updated the modified lines in the ocr document: modified line count :" + updatedLinesInDocument.Count());
            await _unitOfWork.SaveChangesAsync();
        }
        else
        {
            logger.LogInformation(
                "There were no human modified lines in the ocr document, updating only the document status");
        }

        editDocumentInstance = await _unitOfWork.OcrDocuments.GetByIdAsync(editedOcrDocument.Id);
        if (editDocumentInstance is null)
            throw new ArgumentException("There is no ocr document with id " + editedOcrDocument.Id); 
        editDocumentInstance.Status = OcrDocumentStatus_.Corrected;
        editDocumentInstance.UpdatedAt = DateTime.Now;
        _unitOfWork.OcrDocuments.Update(editDocumentInstance);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapperInstance.Map<OcrDocumentDto?>(editDocumentInstance);
    }
}