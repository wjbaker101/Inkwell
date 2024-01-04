using Data.Records;
using Data.Repositories.Log.Types;
using Data.Types;
using NHibernate.Linq;

namespace Data.Repositories.Log;

public interface ILogRepository
{
    Task<SearchLogsDto> Search(SearchLogsParameters parameters, CancellationToken cancellationToken);
    Task<LogRecord> GetByReference(Guid reference, CancellationToken cancellationToken);
    Task<LogRecord> Save(LogRecord log, CancellationToken cancellationToken);
    Task Delete(LogRecord log, CancellationToken cancellationToken);
    Task<List<string>> GetAppNames(CancellationToken cancellationToken);
}

public sealed class LogRepository : ILogRepository
{
    private readonly IDatabase _database;

    public LogRepository(IDatabase database)
    {
        _database = database;
    }

    public async Task<SearchLogsDto> Search(SearchLogsParameters parameters, CancellationToken cancellationToken)
    {
        using var session = _database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var query = session
            .Query<LogRecord>();

        if (parameters.AppName != null)
            query = query.Where(x => x.AppName == parameters.AppName);

        var count = query.ToFutureValue(x => x.Count());

        var logs = (await query
            .Skip(parameters.PageSize * (parameters.PageNumber - 1))
            .Take(parameters.PageSize)
            .OrderByDescending(x => x.CreatedAt)
            .ToFuture()
            .GetEnumerableAsync(cancellationToken))
            .ToList();

        await transaction.CommitAsync(cancellationToken);

        return new SearchLogsDto
        {
            Logs = logs,
            TotalCount = count.Value
        };
    }

    public async Task<LogRecord> GetByReference(Guid reference, CancellationToken cancellationToken)
    {
        using var session = _database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var log = await session
            .Query<LogRecord>()
            .SingleOrDefaultAsync(x => x.Reference == reference, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return log;
    }

    public async Task<LogRecord> Save(LogRecord log, CancellationToken cancellationToken)
    {
        using var session = _database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.SaveAsync(log, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return log;
    }

    public async Task Delete(LogRecord log, CancellationToken cancellationToken)
    {
        using var session = _database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        await session.DeleteAsync(log, cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    public async Task<List<string>> GetAppNames(CancellationToken cancellationToken)
    {
        using var session = _database.SessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        var appNames = await session
            .Query<LogRecord>()
            .Select(x => x.AppName)
            .OrderBy(x => x)
            .Distinct()
            .ToListAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return appNames;
    }
}