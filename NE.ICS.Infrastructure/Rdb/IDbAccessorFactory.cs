using NE.ICS.Infrastructure.Policies;
using System.Collections.Generic;
using System.Data;

namespace NE.ICS.Infrastructure
{
    public interface IDbAccessorFactory<TConnection> where TConnection : IDbConnection, new()
    {
        DbAccessorConfig Config { get; set; }
        IReadWritePolicy CreateReadWritePolicy();

        IInjectionDefensePolicy CreateInjectionDefensePolicy();        

        IDbAccessor<TConnection> CreateDbAccessor(params string[] connectionNames);

        IDbAccessor<TConnection> CreateDbAccessor(IEnumerable<string> connectionNames);

        IDbAccessor<TConnection> CreateDbAccessor(params ConnectionString[] connectionStrings);

        IDbAccessor<TConnection> CreateDbAccessor(IEnumerable<ConnectionString> connectionStrings);


    }
}
