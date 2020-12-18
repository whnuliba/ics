using System;
using System.Collections.Generic;

namespace NE.ICS.Infrastructure
{
    public class ConnectionStrings { 
       public List<ConnectionString> ConnectionStrs { set; get; }
    }
    /// <summary>
    /// ConnectionString.
    /// sample: add name="Name:ReadWrite" connectionString="Data Source=...;Initial Catalog=MyDb;Integrated Security=True" providerName="System.Data.SqlClient"
    /// </summary>
    public class ConnectionString
    {        
        public string Name { get; private set; }

        public string Value { get; private set; }

        public ReadWriteIntent Intent { get; private set; }

        public string ProviderName { get; private set; }

        public ConnectionString()
        {

        }

        public ConnectionString(string name, string value, string providerName = null)
        {
            Name = name;
            Value = value;
            ProviderName = providerName;
            var array = name.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length >1 )
            {
                string flag = array[1].ToLower();
                switch (flag)
                {
                    case "readonly":
                        Intent = ReadWriteIntent.ReadOnly;
                        break;
                    case "readwrite":
                        Intent = ReadWriteIntent.ReadWrite;
                        break;
                    //case "combine":
                    //    Intent = ReadWriteIntent.Combine;
                    //    break;
                    default:
                        Intent = ReadWriteIntent.ReadWrite;
                        break;
                }
            }
            else
            {
                Intent = ReadWriteIntent.ReadWrite;
            }
        }

        public ConnectionString Clone()
        {
            return new ConnectionString { Name = Name, Value = Value, ProviderName = ProviderName, Intent = Intent };
        }
    }
}
