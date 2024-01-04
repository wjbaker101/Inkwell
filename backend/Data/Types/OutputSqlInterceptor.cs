using NHibernate;
using NHibernate.SqlCommand;
using System.Diagnostics;

namespace Data.Types;

public sealed class OutputSqlInterceptor : EmptyInterceptor
{
    public override SqlString OnPrepareStatement(SqlString sql)
    {
        Debug.WriteLine(sql);

        return base.OnPrepareStatement(sql);
    }
}