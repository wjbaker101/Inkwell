namespace Api.Models;

public sealed class InkwellLogModel
{
    public required Guid Reference { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required InkwellLogLevel InkwellLogLevel { get; init; }
    public required string AppName { get; init; }
    public required string Message { get; init; }
    public required string? StackTrace { get; init; }
    public required string? JsonData { get; init; }
}

public enum InkwellLogLevel
{
    Unknown = 0,
    Info = 1,
    Error = 2
}