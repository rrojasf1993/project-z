using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;

namespace HandwritenRecognition.Services;

public interface IDocumentService
{
    public List<OcrDocumentDto> GetOcrDocumentsByStatus(OcrDocumentStatus_ status);
}