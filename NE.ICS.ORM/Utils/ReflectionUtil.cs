using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NE.ICS.ORM.Utils
{
    public class ReflectionUtil
    {
        //通过全类名和程序集获取到对象
        //通过属性名获取到属性
        //通过对象获取到所有的属性和值
        public static object GetFieldValue(string fieldName,Type type, object obj) {
            PropertyInfo[] pis = type.GetProperties();
            foreach (PropertyInfo p in pis)
            {
                if (p.Name == fieldName)
                {
                    return p.GetValue(obj);
                }
            }
            return null;
        }
        public static  object GetFieldValue(string fieldName, object obj) {
            if (string.IsNullOrWhiteSpace(fieldName))
                return null;
            Type Ts = obj.GetType();
            string[] fields = fieldName.Split('.');
            object o = null;

            if (fields.Length ==1 ) {
                return GetFieldValue(fields[0], Ts, obj);
                //return  Ts.GetField(fields[]).GetValue(obj);

            }
            for (int i= 0; i < fields.Length; i++) {
                object _o = GetFieldValue(fields[i -1 ], Ts, obj); 
                o = GetFieldValue(fields[i], _o);
            }
            return o;
        }
        public static MethodInfo GetMethod(string methodName, object obj) {
            Type Ts = obj.GetType();
            return Ts.GetMethod(methodName);
        }

        public static object GetObject(string assemblyName,string fullName) {
            //Assembly assembly = Assembly.Load(assemblyName);
            //Type type = assembly.GetType(fullName);
            object obj = Assembly.Load(assemblyName).CreateInstance(fullName);
            return obj;
        }
        public static object GetObject(string type)
        {
            string [] types = type.Split(',');
            object obj = GetObject(types[1], types[0]);
            return obj;
        }

        public static Type GetType(string assemblyName, string fullName) {
            Assembly assembly = Assembly.Load(assemblyName);
            return  assembly.GetType(fullName);
        }
        public static Type GetType(string type)
        {
            string[] types = type.Split(',');
           return GetType(types[1], types[0]);
        }

        public static string TypeName(object obj) {
            Type type = obj.GetType();
            return $"{type.FullName},{type.Assembly.FullName.Split(',')[0]}";
        }

        public static string TypeName(Type type)
        {
            Assembly assembly = Assembly.GetAssembly(type);
            return $"{type.FullName},{type.Assembly.FullName.Split(',')[0]}";
        }
        public static T ConvertType<T>(object val){
            if (val == null) return default(T);//返回类型的默认值
            Type tp = typeof(T);
            //泛型Nullable判断，取其中的类型
            if (tp.IsGenericType)
            {
                tp = tp.GetGenericArguments()[0];
            }
            //string直接返回转换
            if (tp.Name.ToLower() == "string")
            {
                return (T)val;
            }
            //反射获取TryParse方法
            var TryParse = tp.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
                                            new Type[] { typeof(string), tp.MakeByRefType() },
                                            new ParameterModifier[] { new ParameterModifier() });
            var parameters = new object[] { val, Activator.CreateInstance(tp) };
            bool success = (bool)TryParse.Invoke(null, parameters);
            //成功返回转换后的值，否则返回类型的默认值
            if (success)
            {
                return (T)parameters[0];
            }
            return default(T);
        }

    }
}
