﻿using Api.Models;

namespace Inkwell.Api.Logger.Types;

public sealed class CreateLogRequest
{
    public required InkwellLogLevel LogLevel { get; init; }
    public required string AppName { get; init; }
    public required string Message { get; init; }
    public required string? StackTrace { get; init; }
    public required string? JsonData { get; init; }
}

public sealed class CreateLogResponse
{
    public required InkwellLogModel Log { get; init; }
}