using System.Collections.Concurrent;
using HandwritenRecognition.Cross.DataTransferObjects;

namespace HandwritenRecognition.Cross.Infrastructure;

public static class OcrJobStore
{
    public static ConcurrentDictionary<Guid, OcrJobDto> Jobs { get; } = new ConcurrentDictionary<Guid, OcrJobDto>();
}