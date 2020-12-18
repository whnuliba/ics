using Dapper;
using NE.ICS.Infrastructure.Policies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace NE.ICS.Infrastructure
{
    public class DbAccessor<TConnection> : IDbAccessor<TConnection> where TConnection : IDbConnection, new()
    {
        private string _readWriteConnectionString;
        private string _readOnlyConnectionString;

        public IReadWritePolicy ReadWritePolicy { get; private set; }

        public IInjectionDefensePolicy InjectionDefensePolicy { get; private set; }

        public bool AutoDetectReadOnlyQuery { get; set; }

        
        public DbAccessor(IReadWritePolicy readWritePolicy, IInjectionDefensePolicy injectionDefensePolicy = null)
        {
            ReadWritePolicy = readWritePolicy;
            InjectionDefensePolicy = injectionDefensePolicy;

            _readWriteConnectionString = ReadWritePolicy.GetConnections().FirstOrDefault((c) => c.Intent == ReadWriteIntent.ReadWrite)?.Value;
            _readOnlyConnectionString = ReadWritePolicy.GetConnections().FirstOrDefault((c) => c.Intent == ReadWriteIntent.ReadOnly)?.Value;

            if (_readWriteConnectionString == null && _readOnlyConnectionString == null)
            {
                throw new DbAccessExeception("DbAccessor has no connection strings configured.");
            }
            _readWriteConnectionString = _readWriteConnectionString ?? _readOnlyConnectionString;
            _readOnlyConnectionString = _readOnlyConnectionString ?? _readWriteConnectionString;

        }

        public TConnection CreateConnection(ReadWriteIntent intent = ReadWriteIntent.ReadWrite, bool open = false)
        {
            var connectionString = intent == ReadWriteIntent.ReadWrite ? _readWriteConnectionString : _readOnlyConnectionString;
            var connection = new TConnection { ConnectionString = connectionString };
            if (open)
            {
                connection.Open();
            }
            return connection;
        }


        public int Execute(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {            
            return Invoke(SqlMapper.Execute, sql, param, transational, commandTimeout, commandType);
        }

        public async Task<int> ExecuteAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.ExecuteAsync, sql, param, transational, commandTimeout, commandType);
        }        

        public IDataReader ExecuteReader(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return Invoke(SqlMapper.ExecuteReader, sql, param, transational, commandTimeout, commandType);
        }

        public async Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.ExecuteReaderAsync, sql, param, transational, commandTimeout, commandType);
        }

        public object ExecuteScalar(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {            
            return Invoke(SqlMapper.ExecuteScalar, sql, param, transational, commandTimeout, commandType);
        }

        public async Task<object> ExecuteScalarAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.ExecuteScalarAsync, sql, param, transational, commandTimeout, commandType);
        }

        public T ExecuteScalar<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {            
            return Invoke(SqlMapper.ExecuteScalar<T>, sql, param, transational, commandTimeout, commandType);
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.ExecuteScalarAsync<T>, sql, param, transational, commandTimeout, commandType);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return Invoke(SqlMapper.Query, sql, param, transational, commandTimeout, commandType);
        }

        public object Query(Type type,string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return InvokeAsync(type, sql, param, transational, commandTimeout, commandType);
        }

        public async Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.QueryAsync, sql, param, transational, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return Invoke(SqlMapper.Query<T>, sql, param, transational, commandTimeout, commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.QueryAsync<T>, sql, param, transational, commandTimeout, commandType);
        }

        public dynamic QueryFirst(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return Invoke(SqlMapper.QueryFirst, sql, param, transational, commandTimeout, commandType);
        }

        
        public async Task<dynamic> QueryFirstAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.QueryFirstAsync<dynamic>, sql, param, transational, commandTimeout, commandType);
        }
        
        public T QueryFirst<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return Invoke(SqlMapper.QueryFirst<T>, sql, param, transational, commandTimeout, commandType);
        }

        public async Task<T> QueryFirstAsync<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.QueryFirstAsync<T>, sql, param, transational, commandTimeout, commandType);
        }

        public dynamic QueryFirstOrDefault(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return Invoke(SqlMapper.QueryFirstOrDefault, sql, param, transational, commandTimeout, commandType);
        }

        
        public async Task<dynamic> QueryFirstOrDefaultAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.QueryFirstOrDefaultAsync<dynamic>, sql, param, transational, commandTimeout, commandType);
        }
        
        public T QueryFirstOrDefault<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return Invoke(SqlMapper.QueryFirstOrDefault<T>, sql, param, transational, commandTimeout, commandType);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.QueryFirstOrDefaultAsync<T>, sql, param, transational, commandTimeout, commandType);
        }

        public dynamic QuerySingle(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return Invoke(SqlMapper.QuerySingle, sql, param, transational, commandTimeout, commandType);
        }

        
        public async Task<dynamic> QuerySingleAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.QuerySingleAsync<dynamic>, sql, param, transational, commandTimeout, commandType);
        }

        public T QuerySingle<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return Invoke(SqlMapper.QuerySingle<T>, sql, param, transational, commandTimeout, commandType);
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.QuerySingleAsync<T>, sql, param, transational, commandTimeout, commandType);
        }

        public dynamic QuerySingleOrDefault(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return Invoke(SqlMapper.QuerySingleOrDefault, sql, param, transational, commandTimeout, commandType);
        }

        public async Task<dynamic> QuerySingleOrDefaultAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.QuerySingleOrDefaultAsync<dynamic>, sql, param, transational, commandTimeout, commandType);
        }

        public T QuerySingleOrDefault<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return Invoke(SqlMapper.QuerySingleOrDefault<T>, sql, param, transational, commandTimeout, commandType);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.QuerySingleOrDefaultAsync<T>, sql, param, transational, commandTimeout, commandType);
        }

        public GridReader QueryMultiple(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return Invoke(SqlMapper.QueryMultiple, sql, param, transational, commandTimeout, commandType);
        }
        public async Task<GridReader> QueryMultipleAsync(string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            return await InvokeAsync(SqlMapper.QueryMultipleAsync, sql, param, transational, commandTimeout, commandType);
        }

        // T Invoke<T>
        private T Invoke<T>(Func<IDbConnection, string, object, IDbTransaction, int?, CommandType?, T> func, string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?), [CallerMemberName]string actionName = "")
        {
            // 按照Dapper规则，query语句都必须为参数化查询语句
            // Dapper 会对 params 进行参数格式化，因此不需要进行注入校验。
            //DetectInjection(sql, null, null);
            var connectionString = GetConnectionString(actionName, sql, param);
            return InvokeCore<T>(connectionString, func, sql, param, transational, commandTimeout, commandType);
        }

        private async Task<T> InvokeAsync<T>(Func<IDbConnection, string, object, IDbTransaction, int?, CommandType?, Task<T>> func, string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?), [CallerMemberName]string actionName = "")
        {
            // 按照Dapper规则，query语句都必须为参数化查询语句
            // Dapper 会对 params 进行参数格式化，因此不需要进行注入校验。
            //DetectInjection(sql, null, null);
            var connectionString = GetConnectionString(actionName, sql, param);
            return await InvokeCoreAsync<T>(connectionString, func, sql, param, transational, commandTimeout, commandType);
        }

        private object InvokeAsync(Type type, string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?), [CallerMemberName] string actionName = "")
        {
            // 按照Dapper规则，query语句都必须为参数化查询语句
            // Dapper 会对 params 进行参数格式化，因此不需要进行注入校验。
            //DetectInjection(sql, null, null);
            var connectionString = GetConnectionString(actionName, sql, param);
            return InvokeCoreAsync(connectionString, type, sql, param, transational, commandTimeout, commandType);
        }

        // IEnumerable<T> Invoke<T>
        private IEnumerable<T> Invoke<T>(Func<IDbConnection, string, object, IDbTransaction, bool, int?, CommandType?, IEnumerable<T>> func, string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?), [CallerMemberName]string actionName = "")
        {
            // 按照Dapper规则，query语句都必须为参数化查询语句
            // Dapper 会对 params 进行参数格式化，因此不需要进行注入校验。
            //DetectInjection(sql, null, null);
            var connectionString = GetConnectionString(actionName, sql, param);
            return InvokeCore(connectionString, func, sql, param, transational, commandTimeout, commandType);
        }

        private async Task<IEnumerable<T>> InvokeAsync<T>(Func<IDbConnection, string, object, IDbTransaction, bool, int?, CommandType?, Task<IEnumerable<T>>> func, string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?), [CallerMemberName]string actionName = "")
        {
            // 按照Dapper规则，query语句都必须为参数化查询语句
            // Dapper 会对 params 进行参数格式化，因此不需要进行注入校验。
            //DetectInjection(sql, null, null);
            var connectionString = GetConnectionString(actionName, sql, param);
            return await InvokeCoreAsync(connectionString, func, sql, param, transational, commandTimeout, commandType);
        }

        // Get ConnectionString by actionName ,sql and param.
        private string GetConnectionString(string actionName, string sql, object param = null)
        {
            if (AutoDetectReadOnlyQuery)
            {
                if (ReadWritePolicy.IsReadOnlyQuery(sql) == true)
                {
                    return _readOnlyConnectionString;
                }                    
            }
            var intent = ReadWritePolicy.ValidateAction(actionName);
            return intent == ReadWriteIntent.ReadWrite ? _readWriteConnectionString : _readOnlyConnectionString;

        }

        // Detect Injection
        private void DetectInjection(string whereClause, string orderBy, IEnumerable parameters = null)
        {
            if (InjectionDefensePolicy != null)
            {                
                if (InjectionDefensePolicy.DetectInjection(whereClause, orderBy, null))
                {
                    throw new DbAccessExeception("Sql Injection Detected.");
                }
            }
        }

        private static T InvokeCore<T>(string connectionString, Func<IDbConnection, string, object, IDbTransaction, int?, CommandType?, T> func, string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var connection = new TConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                IDbTransaction transation = null;
                if (transational)
                    transation = connection.BeginTransaction();
                try
                {
                    var result = func(connection, sql, param, transation, commandTimeout, commandType);
                    if (transation != null)
                        transation.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    if (transation != null)
                        transation.Rollback();
                    throw ex;
                }
            }
        }

        private static IEnumerable<object> InvokeCoreAsync(string connectionString,Type type, string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var connection = new TConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                IDbTransaction transation = null;
                if (transational)
                    transation = connection.BeginTransaction();
                try
                {
                    var result = connection.Query(type, sql, param, transation, true, null, commandType);
                    if (transation != null)
                        transation.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    if (transation != null)
                        transation.Rollback();
                    throw ex;
                }
            }
        }
        private static async Task<T> InvokeCoreAsync<T>(string connectionString, Func<IDbConnection, string, object, IDbTransaction, int?, CommandType?, Task<T>> func, string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var connection = new TConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                IDbTransaction transation = null;
                if (transational)
                    transation = connection.BeginTransaction();
                try
                {
                    var result =  await func(connection, sql, param, transation, commandTimeout, commandType);
                    if (transation != null)
                        transation.Commit();
                    return (T)result;
                }
                catch (Exception ex)
                {
                    if (transation != null)
                        transation.Rollback();
                    throw ex;
                }
            }
        }

        private static IEnumerable<T> InvokeCore<T>(string connectionString, Func<IDbConnection, string, object, IDbTransaction, bool, int?, CommandType?, IEnumerable<T>> func, string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var connection = new TConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                IDbTransaction transation = null;
                if (transational)
                    transation = connection.BeginTransaction();
                try
                {
                    var result = func(connection, sql, param, transation, true, commandTimeout, commandType);
                    if (transation != null)
                        transation.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    if (transation != null)
                        transation.Rollback();
                    throw ex;
                }
            }
        }

        private static async Task<IEnumerable<T>> InvokeCoreAsync<T>(string connectionString, Func<IDbConnection, string, object, IDbTransaction, bool, int?, CommandType?, Task<IEnumerable<T>>> func, string sql, object param = null, bool transational = false, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var connection = new TConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                IDbTransaction transation = null;
                if (transational)
                    transation = connection.BeginTransaction();
                try
                {
                    var result = await func(connection, sql, param, transation, true, commandTimeout, commandType);
                    if (transation != null)
                        transation.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    if (transation != null)
                        transation.Rollback();
                    throw ex;
                }
            }
        }        

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    ReadWritePolicy = null;
                    InjectionDefensePolicy = null;
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DbAccessor() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
