using HandwritenRecognition.Data.UnitOfWork;
using HandwritenRecognition.Services;

namespace HandwritenRecognition.API.BackgroundWorkers;


public class PythonOcrWorker : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _svcScopeFactory;
    public PythonOcrWorker(ILogger<PythonOcrWorker> logger, IConfiguration _configuration, IServiceScopeFactory factory)
    {
        this._configuration = _configuration;
        this._logger = logger;
        this._svcScopeFactory = factory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int delayTime = _configuration.GetValue<int>("OcrWorkerSettings:TimeBetweenRunsInMinutes");
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _svcScopeFactory.CreateScope();
            var ocrServiceInstance = scope.ServiceProvider.GetService<IOcrService>();
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("PythonOcrWorker running at: {time}", DateTimeOffset.Now);
                if (ocrServiceInstance is null)
                    return;
                await ocrServiceInstance.UpdateJobResults();
            }
            await Task.Delay(TimeSpan.FromMinutes(delayTime), stoppingToken);
        }
    }

    private Task MakeOcrRequest()
    {
        throw new NotImplementedException();
        ///this._unitOfWork.OcrJobs.
    }
    
    
}