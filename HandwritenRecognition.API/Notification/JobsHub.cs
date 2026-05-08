using HandwritenRecognition.Cross.DataTransferObjects;
using Microsoft.AspNetCore.SignalR;

namespace HandwritenRecognition.API.Notification;

public class JobsHub:Hub<OcrJobDto>
{
    ILogger<JobsHub> _logger;
    public JobsHub(ILogger<JobsHub> loggerInstance)
    {
        _logger = loggerInstance;
    }
    public override Task OnConnectedAsync()
    {
        _logger.LogInformation($"OnConnectedAsync: ConnectionId:{Context.ConnectionId} ");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation($"OnDisconnectedAsync: ConnectionId:{Context.ConnectionId} ");
        if(exception != null)
            _logger.LogError(exception, "OnDisconnectedAsync Exception");
        return base.OnDisconnectedAsync(exception);
    }
}