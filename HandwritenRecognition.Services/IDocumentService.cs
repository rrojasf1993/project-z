using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;

namespace HandwritenRecognition.Services;

public interface IDocumentService
{
    public List<OcrDocumentDto>? GetOcrDocumentsByStatus(OcrDocumentStatus_ status, DateTime? startDate, DateTime? endDate);

    public Task<OcrDocumentDto?> UpdateOcrDocument_WithHumanCorrections(OcrDocumentDto editedOcrDocument);
}