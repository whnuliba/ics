using System.Data;

namespace NE.ICS.Infrastructure
{
    public static class DbAccessorManager
    {
        private static readonly object _sync = new object();
        private static bool _configured;

        public static void Configure(string configFile = "Data.config")
        {
            lock(_sync)
            {                
                ConnectionStringsManager.Configure(configFile);
                _configured = true;
            }
            

        }

        public static IDbAccessorFactory<TConnection> CreateDbAccessorFactory<TConnection>(DbAccessorConfig config = null) where TConnection : IDbConnection, new()
        {
            if (!_configured)
            {
                Configure();
            }

            var factory = new DefaultDbAccessorFactory<TConnection>();
            factory.Config = config ?? new DbAccessorConfig();
            return factory;
        }
        

        public static T CreateDbAccessorFactory<T, TConnection>(DbAccessorConfig config = null) where T : IDbAccessorFactory<TConnection>, new() where TConnection : IDbConnection, new()
        {
            if (!_configured)
            {
                Configure();
            }

            var factory = new T();
            factory.Config = config ?? new DbAccessorConfig();
            return factory;
        }
    }
}
