using Inkwell.Client.Types;
using System.Text;
using System.Text.Json;

namespace Inkwell.Client;

public interface IInkwellClient
{
    Task Log(CreateLogRequest request);
}

public sealed class InkwellClient : IInkwellClient
{
    private readonly InkwellClientOptions _options;

    private readonly HttpClient _httpClient = new();

    public InkwellClient(InkwellClientOptions options)
    {
        _options = options;
    }

    public async Task Log(CreateLogRequest request)
    {
        try
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));

            var requestBody = JsonSerializer.Serialize(new
            {
                _options.AppName,
                request.LogLevel,
                request.Message,
                request.StackTrace,
                JsonData = request.JsonData != null ? JsonSerializer.Serialize(request.JsonData) : null
            });

            var message = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_options.BaseUrl}/api/logger/logs"),
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            await _httpClient.SendAsync(message, cancellationTokenSource.Token);
        }
        catch
        {
            // Do nothing
        }
    }
}
