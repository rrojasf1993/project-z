namespace HandwritenRecognition.Services;

public interface IOcrService
{
    Task<Guid> CreateOcrJob(IFormFile _file);
}