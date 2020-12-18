using NE.ICS.Infrastructure.Policies;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;

namespace NE.ICS.Infrastructure
{
    public class DefaultDbAccessorFactory<TConnection> : IDbAccessorFactory<TConnection> where TConnection : IDbConnection, new()
    {
        public DbAccessorConfig Config { get; set; }

        public IReadWritePolicy CreateReadWritePolicy()
        {
            return new DefaultReadWritePolicy();
        }

        public IInjectionDefensePolicy CreateInjectionDefensePolicy()
        {
            return new DefaultInjectionDefensePolicy(Config.SqlRegex);
        }        

        public IDbAccessor<TConnection> CreateDbAccessor(params string[] connectionNames)
        {
            return CreateDbAccessor((IEnumerable<string>)connectionNames);
        }

        public IDbAccessor<TConnection> CreateDbAccessor(IEnumerable<string> connectionNames)
        {
            var coll = new Collection<ConnectionString>();
            foreach (var connectionName in connectionNames)
            {
                var connectionString = ConnectionStringsManager.GetConnectionString(connectionName);
                if (connectionString == null)
                {
                    connectionString = DefaultConfigurationManager.GetConnectionString(connectionName);
                    if (connectionString == null)
                    {
                        throw new DbAccessExeception($"Connection name [{connectionName}] is not existed.");
                    }                    
                  
                    // 不添加默认配置文件中的链接到ConnectionStringsManager
                    //ConnectionStringsManager.AddConnectionString(connectionString);
                }
                coll.Add(connectionString);
            }           

            return CreateDbAccessor(coll);
        }

        public IDbAccessor<TConnection> CreateDbAccessor(params ConnectionString[] connectionStrings)
        {
            return CreateDbAccessor((IEnumerable<ConnectionString>)connectionStrings);
        }

        public IDbAccessor<TConnection> CreateDbAccessor(IEnumerable<ConnectionString> connectionStrings)
        {
            var readWritePolicy = CreateReadWritePolicy();
            readWritePolicy.DefaultConnectionIntent = Config.DefaultConnectionIntent;
            readWritePolicy.DefaultActionIntent = Config.DefaultActionIntent;
            readWritePolicy.RegisterConnections(connectionStrings);

            var injectionDefensePolicy = CreateInjectionDefensePolicy();
            injectionDefensePolicy.DetectInjectionWithParameters = Config.DelectInjectionWithParameters;

            var dbAccessor = new DbAccessor<TConnection>(readWritePolicy, injectionDefensePolicy);
            dbAccessor.AutoDetectReadOnlyQuery = Config.AutoDetectReadOnlyQuery;
            
            return dbAccessor;
        }
    }
}
