using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NE.ICS.Infrastructure
{
    public class DefaultConfigurationManager
    {
        //Directory.GetCurrentDirectory()
        public static IConfigurationRoot CreateConfig(string path,string file) {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile(file);
            var config =  builder.Build();
            return config;
        }
        public static IConfigurationRoot CreateConfig(string file) {
            return CreateConfig(Directory.GetCurrentDirectory(), file);
        }

        public static IConfigurationRoot CreateConfig()
        {
            return CreateConfig("appsettings.json");
        }

        public static string GetValue(string keyPath,string file=null) {
            IConfigurationRoot config = null;
            if (!string.IsNullOrWhiteSpace(file))
                config = CreateConfig(file);
            else
                config = CreateConfig();
            return config.GetSection(keyPath).Value;
        }

        public static ConnectionString GetConnectionString(string keyPath,string file=null)
        {
            IConfigurationRoot config = null;
            if (!string.IsNullOrWhiteSpace(file))
                config = CreateConfig(file);
            else
                config = CreateConfig();
            var connStr = config.GetSection("ConnectionStrings");
            string name = connStr.GetSection($"{keyPath}:Name").Value;
            string value = connStr.GetSection($"{keyPath}:Value").Value;
            string providerName = connStr.GetSection($"{keyPath}:ProviderName").Value;
            return new ConnectionString(name, value, providerName);
        }

        //public List<T> GetConnectionStrings<T>(T t,string keyPath, string file = null) {
        //    IConfigurationRoot config = null;
        //    if (!string.IsNullOrWhiteSpace(file))
        //        config = CreateConfig(file);
        //    else
        //        config = CreateConfig();
        //    IConfigurationSection myArraySection = config.GetSection(keyPath);
        //    var itemArray = myArraySection.AsEnumerable();
        //    return itemArray;
        //} 
    }
}
