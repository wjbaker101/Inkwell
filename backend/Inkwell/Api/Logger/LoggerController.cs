using DotNetLibs.Api.Types;
using Inkwell.Api.Logger.Types;
using Microsoft.AspNetCore.Mvc;

namespace Inkwell.Api.Logger;

[Route("api/logger")]
public sealed class LoggerController : ApiController
{
    private readonly ILoggerService _loggerService;

    public LoggerController(ILoggerService loggerService)
    {
        _loggerService = loggerService;
    }

    [HttpGet]
    [Route("logs/search")]
    public async Task<IActionResult> SearchLogs(
        [FromQuery(Name = "page_number")] int pageNumber,
        [FromQuery(Name = "page_size")] int pageSize,
        [FromQuery(Name = "app_name")] string? appName,
        CancellationToken cancellationToken)
    {
        var result = await _loggerService.SearchLogs(new SearchLogsRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            AppName = appName
        }, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpPost]
    [Route("logs")]
    public async Task<IActionResult> CreateLog([FromBody] CreateLogRequest request, CancellationToken cancellationToken)
    {
        var result = await _loggerService.CreateLog(request, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpDelete]
    [Route("logs/{logReference:guid}")]
    public async Task<IActionResult> DeleteLog([FromRoute] Guid logReference, CancellationToken cancellationToken)
    {
        var result = await _loggerService.DeleteLog(logReference, cancellationToken);

        return ToApiResponse(result);
    }

    [HttpGet]
    [Route("app-names")]
    public async Task<IActionResult> GetAppNames(CancellationToken cancellationToken)
    {
        var result = await _loggerService.GetAppNames(cancellationToken);

        return ToApiResponse(result);
    }
}