using Api.Models;

namespace Inkwell.Api.Logger.Types;

public sealed class SearchLogsRequest
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required string? AppName { get; init; }
}

public sealed class SearchLogsResponse
{
    public required List<InkwellLogModel> Logs { get; init; }
    public required int TotalCount { get; init; }
    public required int TotalPages { get; init; }
}