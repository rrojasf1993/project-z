using HandwritenRecognition.Cross.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace HandwritenRecognition.Services;

public interface IOcrService
{
    Task<OcrJobDto?> CreateOcrJob(IFormFile _file);
    Task<OcrJobDto?> CreateBatchOcrJob(IFormFileCollection _files);
    public  Task UpdateJobResults();
}