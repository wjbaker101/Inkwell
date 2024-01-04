using Data.Records;

namespace Data.Repositories.Log.Types;

public sealed class SearchLogsParameters
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required string? AppName { get; init; }
}

public sealed class SearchLogsDto
{
    public required List<LogRecord> Logs { get; init; }
    public required int TotalCount { get; init; }
}