using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace NE.ICS.Infrastructure
{
    public static class ConnectionStringsManager
    {
        private static readonly object _sync = new object();
        //private static bool _configured;
        private static readonly ConcurrentDictionary<string, ConnectionString> _dict = new ConcurrentDictionary<string, ConnectionString>();

        public static void Configure(string configFile = null, bool clear = false)
        {
            lock (_sync)
            {
                var fullFileName = GetFullFileName(configFile);
                if (fullFileName != null)
                {
                    //var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = fullFileName };
                    //var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                    //Load(configuration.ConnectionStrings.ConnectionStrings, clear);
                }
                else
                {
                    // 默认配置文件的链接字符串不加载到字段集合中
                    //Load(ConfigurationManager.ConnectionStrings, clear);
                }
                //_configured = true;
            }
            
        }

        //private static void Load(ConnectionStringSettingsCollection connectionStringSettings, bool clear = false)
        //{
        //    if (clear)
        //    {
        //        _dict.Clear();
        //    }
        //    foreach (ConnectionStringSettings settings in connectionStringSettings)
        //    {
        //        var connectionString = new ConnectionString(settings.Name, settings.ConnectionString, settings.ProviderName);
        //        AddConnectionString(connectionString);
        //    }
        //}

        private static string GetFullFileName(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return null;
            }

            if (Path.IsPathRooted(fileName))
            {
                return fileName;
            }

            var fullFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(fullFileName))
            {
                if (null != AppDomain.CurrentDomain.RelativeSearchPath)
                {
                    fullFileName = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, fileName);
                }
                if (!File.Exists(fullFileName))
                {                    
                    return null;
                }
            }

            return fullFileName;
        }

        public static bool ExistsConnectionString(string name)
        {
            return _dict.ContainsKey(name);
        }

        public static ConnectionString GetConnectionString(string name)
        {
            if (_dict.ContainsKey(name))
            {
                return _dict[name];
            }
            return null;    
        }

        public static IEnumerable<ConnectionString> GetConnectionStrings()
        {
            return _dict.Values.ToList();
        }

        public static void AddConnectionString(ConnectionString connectionString)
        {
            _dict[connectionString.Name] = connectionString;
        }

        public static void AddConnectionStrings(IEnumerable<ConnectionString> ConnectionStrings)
        {
            foreach (var connectionString in ConnectionStrings)
            {
                AddConnectionString(connectionString);
            }
        }

        public static void RemoveConnectionString(string name)
        {
            ConnectionString connectionString;
            _dict.TryRemove(name, out connectionString);
        }

        public static void RemoveConnectionStrings(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                RemoveConnectionString(name);
            }
        }

        public static void Clear()
        {
            _dict.Clear();
        }

        
    }
}
