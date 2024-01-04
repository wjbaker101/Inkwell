using Data.Values;
using FluentNHibernate.Mapping;

namespace Data.Records;

public class LogRecord
{
    public virtual long Id { get; init; }
    public virtual required Guid Reference { get; init; }
    public virtual required DateTime CreatedAt { get; init; }
    public virtual required LogLevel LogLevel { get; init; }
    public virtual required string AppName { get; init; }
    public virtual required string Message { get; init; }
    public virtual required string? StackTrace { get; init; }
    public virtual required string? JsonData { get; init; }
}

public enum LogLevel
{
    Unknown = 0,
    Info = 1,
    Error = 2
}

public sealed class LogRecordMap : ClassMap<LogRecord>
{
    public LogRecordMap()
    {
        Schema(DatabaseValues.SCHEMA);
        Table("log");
        Id(x => x.Id, "id").GeneratedBy.SequenceIdentity("log_id_seq");
        Map(x => x.Reference, "reference");
        Map(x => x.CreatedAt, "created_at");
        Map(x => x.LogLevel, "log_level").CustomType<LogLevel>();
        Map(x => x.AppName, "app_name");
        Map(x => x.Message, "message");
        Map(x => x.StackTrace, "stack_trace");
        Map(x => x.JsonData, "json_data");
    }
}