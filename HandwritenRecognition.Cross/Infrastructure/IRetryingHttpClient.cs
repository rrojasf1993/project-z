namespace HandwritenRecognition.Cross.Infrastructure;

public interface IRetryingHttpClient
{
    Task<T?> GetAsync<T>(string uri);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest data, Dictionary<string, string>? headers);

    Task<TResponse?> PostMultipartFormDataAsync<TRequest, TResponse>(string uri, Stream data, string filename,
        Dictionary<string, string>? headers);
}