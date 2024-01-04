﻿using Core.Settings;
using Data.Records;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Data.Types;

public interface IDatabase
{
    ISessionFactory SessionFactory { get; }
}

public sealed class Database : IDatabase
{
    public ISessionFactory SessionFactory { get; }

    public Database(AppSecrets secrets)
    {
        var database = secrets.Database;

        SessionFactory = Fluently.Configure()
            .Database(PostgreSQLConfiguration.Standard.ConnectionString(c => c
                .Host(database.Host)
                .Port(database.Port)
                .Database(database.Database)
                .Username(database.Username)
                .Password(database.Password)))
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<_Records>())
#if DEBUG
            .ExposeConfiguration(x => x.SetInterceptor(new OutputSqlInterceptor()))
#endif
            .BuildSessionFactory();
    }
}