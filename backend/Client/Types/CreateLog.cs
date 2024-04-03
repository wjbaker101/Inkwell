namespace Inkwell.Client.Types;

public sealed class CreateLogRequest
{
    public required InkwellLogLevel LogLevel { get; init; }
    public required string Message { get; init; }
    public required string? StackTrace { get; init; }
    public required object? JsonData { get; init; }
}

public enum InkwellLogLevel
{
    Unknown = 0,
    Info = 1,
    Error = 2
}