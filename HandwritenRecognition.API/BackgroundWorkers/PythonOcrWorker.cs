using HandwritenRecognition.API.Notification;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Cross.Enums;
using HandwritenRecognition.Data.UnitOfWork;
using HandwritenRecognition.Services;
using Microsoft.AspNetCore.SignalR;

namespace HandwritenRecognition.API.BackgroundWorkers;


public class PythonOcrWorker(
    ILogger<PythonOcrWorker> logger,
    IConfiguration configuration,
    IServiceScopeFactory factory,
    IHubContext<JobsHub> hubContext)
    : BackgroundService
{
    private readonly ILogger _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int delayTime = configuration.GetValue<int>("OcrWorkerSettings:TimeBetweenRunsInMinutes");
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = factory.CreateScope();
            var ocrServiceInstance = scope.ServiceProvider.GetService<IOcrService>();
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("PythonOcrWorker running at: {time}", DateTimeOffset.Now);
                if (ocrServiceInstance is null)
                    return;
                var jobIdsToUpdate = ocrServiceInstance.GetPendingJobsFromDatabase().Select(j => j.JobId);
                if (jobIdsToUpdate is null)
                    return;
                OcrResultDto? result = null;
                foreach (var jobId in jobIdsToUpdate)
                {
                    try
                    { 
                        result= await ocrServiceInstance.UpdateJobResultsByJobId(jobId);
                        if (result is null)
                            continue;
                        await hubContext.Clients.All.SendAsync("OcrEvent", new OcrJobEvent()
                        {
                            Type = MessageTypes.OcrRecognitionSuccess,
                            Data = result,
                            TimeStamp = DateTime.Now,
                            JobId = jobId
                        }, cancellationToken: stoppingToken);

                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"PythonOcrWorker Exception while processing ocr job {jobId}");
                        await hubContext.Clients.All.SendAsync("OcrEvent", new OcrJobEvent()
                        {
                            Type =  MessageTypes.OcrRecognitionFailure,
                            JobId = jobId,
                            ErrorDetails =  e.Message,
                            Data = result
                        });
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(delayTime), stoppingToken);
            }
        }
    }
}