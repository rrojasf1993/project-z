using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Cross.Infrastructure;

namespace HandwritenRecognition.Services;

public class OcrService:IOcrService
{ 
    public OcrService(IRetryingHttpClient _retryingHttpClient) //IConfiguration _configuration)
    {
        RetryingHttpClient = _retryingHttpClient;
    }

    private IRetryingHttpClient RetryingHttpClient { get; }

    public async Task<Guid> CreateOcrJob(IFormFile _file)
    {
        var jobId = Guid.NewGuid();
        var newJob = new OcrJobDto
        {
            JobId = jobId,
            Status = OcrJobStatus.Pending
        };
        OcrJobStore.Jobs[jobId] = newJob;

        try
        {
            newJob.Status = OcrJobStatus.Processing;
            var url = "/api/ocr";
            await using (var fs = _file.OpenReadStream())
            {
                ;
            }

            var ocrApiResult =
                await RetryingHttpClient.PostMultipartFormDataAsync<OcrJobDto, OcrResultDto>(url, fs, fs.Name);
            newJob.Status = OcrJobStatus.Completed;
            newJob.Result = ocrApiResult;
        }
        catch (Exception e)
        {
            newJob.Status = OcrJobStatus.Failed;
            newJob.Error = $"{e.Message}\n{e.StackTrace}";
        }

        return newJob.JobId;
    }
}