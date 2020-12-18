using MySql.Data.MySqlClient;
using NE.ICS.Infrastructure;
using NE.ICS.ORM.Utils;
using NE.ICS.ORM.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NE.ICS.ORM
{
    public class MapperMethod
    {
        IDbAccessor<MySqlConnection> dbAccessor = DbAccessorManager.CreateDbAccessorFactory<MySqlConnection>().CreateDbAccessor("db2");
        public object Select(BoundSql sql) {
            Type ts = null;
            if (!string.IsNullOrWhiteSpace(sql.ParameterType)) {
                ts = ReflectionUtil.GetType(sql.ParameterType);
                return dbAccessor.Query(ts,sql.ActualSql, sql.ActualParameters);
            }
            return  dbAccessor.Query(sql.ActualSql, sql.ActualParameters);
        }
        public void Insert()
        {

        }
        public void Update()
        {

        }

        public void Delete()
        {

        }
    }
}
