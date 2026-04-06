using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HandwritenRecognition.Cross.Infrastructure;

public class RetryingHttpClient : IRetryingHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RetryingHttpClient> _loggerInstance;
    //private readonly IAsyncPolicy<HttpResponseMessage> _policy;

    public RetryingHttpClient(HttpClient httpClient,   ILogger<RetryingHttpClient> logger)
    {
        _httpClient = httpClient;
        Console.WriteLine(_httpClient.Timeout.ToString());
        // _policy = policy;
        _loggerInstance = logger;
    }

    public async Task<T?> GetAsync<T>(string uri)
    {
        var response = await /*_policy.ExecuteAsync(() =>*/ _httpClient.GetAsync(uri); //);
        if (!response.IsSuccessStatusCode) return default;

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest data,
        Dictionary<string, string>? headers)
    {
        var stopwatch = Stopwatch.StartNew();
        _loggerInstance.LogInformation("Request started to: {uri} with post method", uri);

        try
        {
            var json = JsonSerializer.Serialize(data);
             _loggerInstance.LogInformation($"Json Input: {json}");

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            if (headers != null)
                _loggerInstance.LogInformation("Request headers: \n");
            if (headers != null)
                foreach (var header in headers)
                {
                    _loggerInstance.LogInformation($"{header.Key}: {header.Value}");
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

            var response = await _httpClient.PostAsync(uri, content);
            stopwatch.Stop();

            var responseContent = await response.Content.ReadAsStringAsync();

            _loggerInstance.LogInformation($"Ended request to : {uri} en {stopwatch.ElapsedMilliseconds} ms");
            _loggerInstance.LogInformation(
             $"Response: {response.StatusCode} - {response.ReasonPhrase}\nContent: {responseContent}\n Response Headers: {response.Headers}\n Input payload: {json}");

            if (!response.IsSuccessStatusCode)
            {
                _loggerInstance.LogError(
                    $"Error in http request  {uri}\nError code: {response.StatusCode}\nReason: {response.ReasonPhrase}");
                return default;
            }

            return JsonSerializer.Deserialize<TResponse>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _loggerInstance.LogError(ex, $"Exception posting to {uri} after {stopwatch.ElapsedMilliseconds} ms");
            throw;
        }
    }

    public async Task<TResponse?> PostMultipartFormDataAsync<TRequest, TResponse>(string uri, Stream data,
        string filename, Dictionary<string, string>? headers)
    {
        var stopwatch = Stopwatch.StartNew();
        //_loggerInstance.LogInformation("Solicitud iniciada a {uri} con método Post", uri);

        try
        {
           // var json = JsonSerializer.Serialize(data);
            // _loggerInstance.LogInformation($"Payload de entrada: {json}");

            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(data), "file", filename);

            if (headers != null)
                //_loggerInstance.LogInformation("Headers de la entrada:");
                foreach (var header in headers)
                    //_loggerInstance.LogInformation($"{header.Key}: {header.Value}");
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);

            var response = await _httpClient.PostAsync(uri, content);
            stopwatch.Stop();

            var responseContent = await response.Content.ReadAsStringAsync();

            //_loggerInstance.LogInformation($"Petición terminada: {uri} en {stopwatch.ElapsedMilliseconds} ms");
            //_loggerInstance.LogInformation(
            //$"Respuesta: {response.StatusCode} - {response.ReasonPhrase}\nContenido: {responseContent}\nEncabezados: {response.Headers}\nDatos de entrada: {json}";

            if (!response.IsSuccessStatusCode)
                /* _loggerInstance.LogError(
                    $"Error haciendo petición a: {uri}\nCódigo de error: {response.StatusCode}\nRazón: {response.ReasonPhrase}");*/
                return default;

            return JsonSerializer.Deserialize<TResponse>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            //_loggerInstance.LogError(ex, $"Excepción al hacer PostMultipartFormDataAsync a {uri} después de {stopwatch.ElapsedMilliseconds} ms");
            return default;
        }
    }
}