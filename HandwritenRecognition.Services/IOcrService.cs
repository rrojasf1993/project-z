using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace HandwritenRecognition.Services;

public interface IOcrService
{
    Task<OcrJobDto?> CreateOcrJob(IFormFile _file);
    Task<OcrJobDto?> CreateBatchOcrJob(IFormFileCollection _files);
    public  Task UpdateJobResults();

    public OcrJobDto GetOcrJobByDocumentId(Guid id);

    List<OcrJobDto> GetOcrJobsByStatus(OcrJobStatus status);
}