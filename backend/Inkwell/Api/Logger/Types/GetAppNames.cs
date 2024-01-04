namespace Inkwell.Api.Logger.Types;

public sealed class GetAppNamesResponse
{
    public required List<string> AppNames { get; init; }
}