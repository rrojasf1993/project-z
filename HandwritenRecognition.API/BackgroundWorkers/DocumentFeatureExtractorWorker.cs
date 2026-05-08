using HandwritenRecognition.Cross;
using HandwritenRecognition.Services;

namespace HandwritenRecognition.API.BackgroundWorkers;

public class DocumentFeatureExtractorWorker(
    ILogger<DocumentFeatureExtractorWorker> logger,
    IConfiguration configuration,
    IServiceScopeFactory factory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int delayTime = configuration.GetValue<int>("FeatureExtractorWorkerSettings:TimeBetweenRunsInMinutes");
        logger.LogInformation("Waiting for {delay} seconds before starting processing", delayTime);
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = factory.CreateScope();
            var documentService=scope.ServiceProvider.GetService<IDocumentService>();
            //var ruleServiceInstance=scope.ServiceProvider.GetService<IRuleService>();
            logger.LogInformation("DocumentFeatureExtractorWorker running at: {time}", DateTimeOffset.Now);
            var documents = documentService?.GetOcrDocumentsByStatus(OcrDocumentStatus_.Corrected, null, null);
            logger.LogInformation("Processing {documentCount}  corrected documents", documents?.Count());
            // var rules = ruleServiceInstance?.GetRules(null);
            // _logger.LogInformation("Retrieved {ruleCount}  rules", rules?.Count());
            if (documents is null)
                return;
            documents.AsParallel().ForAll(document =>
            {
                //correr el motor de reglas?
            });
        }
        await Task.Delay(TimeSpan.FromMinutes(delayTime), stoppingToken);
    }
}