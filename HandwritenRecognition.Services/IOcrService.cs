using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace HandwritenRecognition.Services;

public interface IOcrService
{
    Task<OcrJobDto?> CreateOcrJob(IFormFile _file);
    Task<OcrJobDto?> CreateBatchOcrJob(IFormFileCollection _files);

    public IEnumerable<OcrJobDto> GetPendingJobsFromDatabase();

    public Task<OcrResultDto?> UpdateJobResultsByJobId(Guid jobId);

    public OcrJobDto GetOcrJobByDocumentId(Guid id);

    public Task<OcrJobDto?> GetOcrJobById(Guid id);

    List<OcrJobDto> GetOcrJobsByStatus(OcrJobStatus status, DateTime? startDate, DateTime? endDate);
}