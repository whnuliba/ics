using NE.ICS.Infrastructure.Policies;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace NE.ICS.Infrastructure
{
    public interface IDbAccessor<TConnection> : IDisposable where TConnection : IDbConnection, new()
    {
        IReadWritePolicy ReadWritePolicy { get; }

        IInjectionDefensePolicy InjectionDefensePolicy { get; }

        bool AutoDetectReadOnlyQuery { get; set; }

        TConnection CreateConnection(ReadWriteIntent intent = ReadWriteIntent.ReadWrite, bool open = false);

        int Execute(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<int> ExecuteAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        IDataReader ExecuteReader(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        object ExecuteScalar(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<object> ExecuteScalarAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        T ExecuteScalar<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<T> ExecuteScalarAsync<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        IEnumerable<dynamic> Query(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        public object Query(Type type, string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        IEnumerable<T> Query<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        dynamic QueryFirst(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<dynamic> QueryFirstAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        T QueryFirst<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<T> QueryFirstAsync<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        dynamic QueryFirstOrDefault(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<dynamic> QueryFirstOrDefaultAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        T QueryFirstOrDefault<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        dynamic QuerySingle(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<dynamic> QuerySingleAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        T QuerySingle<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<T> QuerySingleAsync<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        dynamic QuerySingleOrDefault(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<dynamic> QuerySingleOrDefaultAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        T QuerySingleOrDefault<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        GridReader QueryMultiple(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        Task<GridReader> QueryMultipleAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

    }
}
