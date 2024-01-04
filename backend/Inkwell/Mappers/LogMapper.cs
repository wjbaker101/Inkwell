using Api.Models;
using Data.Records;

namespace Inkwell.Mappers;

public static class LogMapper
{
    public static InkwellLogModel Map(LogRecord log) => new()
    {
        Reference = log.Reference,
        CreatedAt = log.CreatedAt,
        InkwellLogLevel = (InkwellLogLevel)log.LogLevel,
        AppName = log.AppName,
        Message = log.Message,
        StackTrace = log.StackTrace,
        JsonData = log.JsonData
    };
}