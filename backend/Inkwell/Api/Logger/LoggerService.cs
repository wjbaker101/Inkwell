using Data.Records;
using Data.Repositories.Log;
using Data.Repositories.Log.Types;
using DotNetLibs.Core.Types;
using Inkwell.Api.Logger.Types;
using Inkwell.Mappers;
using LogLevel = Data.Records.LogLevel;

namespace Inkwell.Api.Logger;

public interface ILoggerService
{
    Task<Result<SearchLogsResponse>> SearchLogs(SearchLogsRequest request, CancellationToken cancellationToken);
    Task<Result<CreateLogResponse>> CreateLog(CreateLogRequest request, CancellationToken cancellationToken);
    Task<Result<DeleteLogResponse>> DeleteLog(Guid logReference, CancellationToken cancellationToken);
    Task<Result<GetAppNamesResponse>> GetAppNames(CancellationToken cancellationToken);
}

public sealed class LoggerService : ILoggerService
{
    private readonly ILogRepository _logRepository;

    public LoggerService(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task<Result<SearchLogsResponse>> SearchLogs(SearchLogsRequest request, CancellationToken cancellationToken)
    {
        var search = await _logRepository.Search(new SearchLogsParameters
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            AppName = request.AppName
        }, cancellationToken);

        return new SearchLogsResponse
        {
            Logs = search.Logs.ConvertAll(LogMapper.Map),
            TotalCount = search.TotalCount,
            TotalPages = search.TotalCount / request.PageSize
        };
    }

    public async Task<Result<CreateLogResponse>> CreateLog(CreateLogRequest request, CancellationToken cancellationToken)
    {
        var log = await _logRepository.Save(new LogRecord
        {
            Reference = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            LogLevel = (LogLevel)request.LogLevel,
            AppName = request.AppName,
            Message = request.Message,
            StackTrace = request.StackTrace,
            JsonData = request.JsonData
        }, cancellationToken);

        return new CreateLogResponse
        {
            Log = LogMapper.Map(log)
        };
    }

    public async Task<Result<DeleteLogResponse>> DeleteLog(Guid logReference, CancellationToken cancellationToken)
    {
        var log = await _logRepository.GetByReference(logReference, cancellationToken);

        await _logRepository.Delete(log, cancellationToken);

        return new DeleteLogResponse();
    }

    public async Task<Result<GetAppNamesResponse>> GetAppNames(CancellationToken cancellationToken)
    {
        var appNames = await _logRepository.GetAppNames(cancellationToken);

        return new GetAppNamesResponse
        {
            AppNames = appNames
        };
    }
}