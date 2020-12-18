using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NE.ICS.Infrastructure.Policies
{
    public class DefaultReadWritePolicy : IReadWritePolicy
    {
        protected readonly ConcurrentDictionary<string, ConnectionString> _connectionStrings = new ConcurrentDictionary<string, ConnectionString>();
        protected readonly ConcurrentDictionary<string, ReadWriteIntent> _actionNames = new ConcurrentDictionary<string, ReadWriteIntent>();        

        public ReadWriteIntent DefaultConnectionIntent { get; set; } = ReadWriteIntent.ReadOnly;

        public ReadWriteIntent DefaultActionIntent { get; set; } = ReadWriteIntent.ReadOnly;


        public DefaultReadWritePolicy() : this(null)
        {
        }        

        public DefaultReadWritePolicy(IEnumerable<ConnectionString> connectionStrings, IDictionary<string, ReadWriteIntent> actionNames = null)
        {
            if (connectionStrings != null)
            {
                foreach (var connectionString in connectionStrings)
                {
                    _connectionStrings[connectionString.Name] = connectionString;
                }
            }            
            if (actionNames != null)
            {
                foreach (var pair in actionNames)
                {
                    actionNames[pair.Key] = pair.Value;
                }
            }
            else
            {
                InitDefaultActionNames();
            }
        }
        

        public bool ExistsConnection(string connectionName)
        {
            return _connectionStrings.ContainsKey(connectionName);
        }

        public ConnectionString GetConnection(string connectionName)
        {
            return _connectionStrings[connectionName];
        }

        public IEnumerable<ConnectionString> GetConnections()
        {
            return _connectionStrings.Values.ToList();
        }

        public void RegisterConnection(ConnectionString connectionString)
        {
            //Override the same connectionString
            _connectionStrings[connectionString.Name] = connectionString;
        }

        public void RegisterConnections(IEnumerable<ConnectionString> connectionStrings)
        {
            foreach (var connectionString in connectionStrings)
            {
                _connectionStrings[connectionString.Name] = connectionString;
            }
        }


        public void RemoveConnection(string connectionName)
        {
            ConnectionString connectionString;
            _connectionStrings.TryRemove(connectionName, out connectionString);
        }


        public void RegisterAction(string actionName, ReadWriteIntent intent)
        {
            //Override the same actionName
            _actionNames[actionName] = intent;
        }


        public void RemoveAction(string actionName)
        {
            ReadWriteIntent intent;
            _actionNames.TryRemove(actionName, out intent);
        }


        public ReadWriteIntent ValidateConnection(string connectionName)
        {
            if (_connectionStrings.ContainsKey(connectionName))
            {
                return _connectionStrings[connectionName].Intent;
            }
            return DefaultConnectionIntent;
        }

        public ReadWriteIntent ValidateAction(string actionName)
        {
            if (_actionNames.ContainsKey(actionName))
            {
                return _actionNames[actionName];
            }                
            return DefaultActionIntent;
        }

        // 默认实现：以 SELECT 开头的 SQL 判定为只读语句 
        public bool IsReadOnlyQuery(string query)
        {
            return query.TrimStart().StartsWith("SELECT ", StringComparison.OrdinalIgnoreCase);
            //return SqlRegex.IsReadOnlyQuery(query);
        }

        // Init Default AtcionNames
        private void InitDefaultActionNames()
        {
            _actionNames[ActionTypes.Execute] = ReadWriteIntent.ReadWrite;
            _actionNames[ActionTypes.ExecuteAsync] = ReadWriteIntent.ReadWrite;
            _actionNames[ActionTypes.ExecuteScalar] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.ExecuteScalarAsync] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.Query] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.QueryAsync] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.QueryFirst] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.QueryFirstAsync] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.QueryFirstOrDefault] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.QueryFirstOrDefaultAsync] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.QuerySingle] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.QuerySingleAsync] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.QuerySingleOrDefault] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.QuerySingleOrDefaultAsync] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.QueryMultiple] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.QueryMultipleAsync] = ReadWriteIntent.ReadOnly;

            _actionNames[ActionTypes.Delete] = ReadWriteIntent.ReadWrite;
            _actionNames[ActionTypes.DeleteAll] = ReadWriteIntent.ReadWrite;
            _actionNames[ActionTypes.Get] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.GetAll] = ReadWriteIntent.ReadOnly;
            _actionNames[ActionTypes.Insert] = ReadWriteIntent.ReadWrite;
            _actionNames[ActionTypes.Update] = ReadWriteIntent.ReadWrite;
        }




    }
}
