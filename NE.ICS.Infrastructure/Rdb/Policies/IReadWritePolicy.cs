using System.Collections.Generic;

namespace NE.ICS.Infrastructure.Policies
{
    public interface IReadWritePolicy
    {
        ReadWriteIntent DefaultConnectionIntent { get; set; }

        ReadWriteIntent DefaultActionIntent { get; set; }

        bool ExistsConnection(string connectionName);

        ConnectionString GetConnection(string connectionName);

        IEnumerable<ConnectionString> GetConnections();

        void RegisterConnection(ConnectionString connectionString);

        void RegisterConnections(IEnumerable<ConnectionString> connectionStrings);

        void RemoveConnection(string connectionName);

        void RegisterAction(string actionName, ReadWriteIntent intent);

        void RemoveAction(string actionName);

        ReadWriteIntent ValidateConnection(string connectionName);

        ReadWriteIntent ValidateAction(string actionName);

        bool IsReadOnlyQuery(string query);

    }
}
