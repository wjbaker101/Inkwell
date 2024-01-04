using Data.Repositories.Log;
using Data.Types;
using Inkwell.Api.Logger;

namespace Inkwell.Setup;

public static class AddDependenciesExtension
{
    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IDatabase, Database>();
        services.AddSingleton<ILogRepository, LogRepository>();

        services.AddSingleton<ILoggerService, LoggerService>();
    }
}