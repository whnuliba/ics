using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NE.ICS.ORM.Xml
{
    public class MetaObject
    {

       // public List<SqlObject> BoundSql = new List<SqlObject>();
        public Dictionary<string, SqlObject> BoundSql = new Dictionary<string, SqlObject>();
        private static object _locker = new object();
        private static MetaObject _instance;


        protected virtual void IniInstance()
        {
        }

        /// <summary>
        /// 取得该类型的实例
        /// </summary>
        public static MetaObject Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new MetaObject();
                            _instance.IniInstance();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
