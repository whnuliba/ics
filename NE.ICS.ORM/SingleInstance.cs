using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.ORM
{
    /// <summary>
    /// 单例实例化基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleInstance<T> where T : SingleInstance<T>, new()
    {
        private static object _locker = new object();
        private static T _instance;

        protected virtual void IniInstance()
        {
        }

        /// <summary>
        /// 取得该类型的实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                            _instance.IniInstance();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
