using DotNetLibs.Api.Types;
using DotNetLibs.Core.Types;
using Inkwell.Client.Types;
using System.Text;
using System.Text.Json;

namespace Inkwell.Client;

public interface IInkwellClient
{
    Task<Result<CreateLogResponse>> Log(CreateLogRequest request, CancellationToken cancellationToken);
}

public sealed class InkwellClient : IInkwellClient
{
    private readonly InkwellClientOptions _options;

    private readonly HttpClient _httpClient = new();

    public InkwellClient(InkwellClientOptions options)
    {
        _options = options;
    }

    public async Task<Result<CreateLogResponse>> Log(CreateLogRequest request, CancellationToken cancellationToken)
    {
        var requestBody = JsonSerializer.Serialize(new
        {
            _options.AppName,
            request.LogLevel,
            request.Message,
            request.StackTrace,
            request.JsonData
        });

        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_options.BaseUrl}/api/logger/logs"),
            Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(message, cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        var responseAsJson = JsonSerializer.Deserialize<ApiResultResponse<CreateLogResponse>>(responseBody);
        if (responseAsJson == null)
            return Result<CreateLogResponse>.Failure("Unable to parse response.");

        return responseAsJson.Result;
    }
}
