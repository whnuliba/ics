using System;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using NE.ICS.ORM.Xml;
using NE.ICS.ORM.Utils;

namespace NE.ICS.ORM.proxy
{
    public class MethodProxy<T> : DispatchProxy
    {
        //关键词 RealProxy
        private T decorated;
        private event Action<MethodInfo, object[]> _afterAction;   //动作之后执行
        private event Action<MethodInfo, object[]> _beforeAction;   //动作之前执行

        //其他自定义属性，事件和方法
        public MethodProxy()
        {

        }
        /// <summary>
        /// 创建代理实例
        /// </summary>
        /// <param name="decorated">代理的接口类型</param>
        /// <returns></returns>
        public T Create(T decorated)
        {
            object proxy = Create<T, MethodProxy<T>>();   //调用DispatchProxy 的Create  创建一个新的T
            ((MethodProxy<T>)proxy).decorated = decorated;       //这里必须这样赋值，会自动未proxy 添加一个新的属性　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　//其他的请如法炮制  　　　　　　　　　　return (T)proxy;
            return (T)proxy;
        }
        public T Create()
        {
            object proxy = Create<T, MethodProxy<T>>();   //调用DispatchProxy 的Create  创建一个新的T
            //((MethodProxy<T>)proxy).decorated = decorated;       //这里必须这样赋值，会自动未proxy 添加一个新的属性　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　//其他的请如法炮制  　　　　　　　　　　return (T)proxy;
            return (T)proxy;
        }
        /// <summary>
        /// 创建代理实例
        /// </summary>
        /// <param name="decorated">代理的接口类型</param>
        /// <param name="beforeAction">方法执行前执行的事件</param>
        /// <param name="afterAction">方法执行后执行的事件</param>
        /// <returns></returns>
        public T Create(T decorated, Action<MethodInfo, object[]> beforeAction, Action<MethodInfo, object[]> afterAction)
        {

            object proxy = Create<T, MethodProxy<T>>();   //调用DispatchProxy 的Create  创建一个新的T
            ((MethodProxy<T>)proxy).decorated = decorated;
            ((MethodProxy<T>)proxy)._afterAction = afterAction;
            ((MethodProxy<T>)proxy)._beforeAction = beforeAction;
            //((GenericDecorator<T>)proxy)._loggingScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            return (T)proxy;
        }



        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod == null) throw new Exception("无效的方法");

            try
            {
                //_beforeAction 事件
                if (_beforeAction != null)
                {
                    this._beforeAction(targetMethod, args);
                }

                object result = null; //targetMethod.Invoke(decorated, args);
                //处理代理后执行逻辑
                #region 代理到Method
                string mName = targetMethod.Name;
                //string type =targetMethod.DeclaringType.Name;
                MetaObject mo = MetaObject.Instance;
                
                if (mo.BoundSql.TryGetValue(ReflectionUtil.TypeName(targetMethod.DeclaringType), out SqlObject sqlObject))
                {
                    if (sqlObject.BoundSql.TryGetValue(mName, out BoundSql bSql))
                    {
                        BoundSqlAnalyzer ba = new BoundSqlAnalyzer(bSql);
                        Object o = null;
                        if (args == null || args.Length == 0)
                            o = new object();
                        else
                            o = args[0];
                        BoundSql sql = ba.AnalyzerSql(o).BoundSql;
                        MapperMethod mm = new MapperMethod();
                        switch (sql.Command) {
                            case XmlSqlEnum.select:
                                result=mm.Select(sql);
                                break;

                        }
                    }

                }
                #endregion


                var resultTask = result as Task;
                if (resultTask != null)
                {
                    resultTask.ContinueWith(task =>    //ContinueWith   创建一个延续，该延续接收调用方提供的状态信息并执行 当目标系统 tasks。 
                    {
                        if (task.Exception != null)
                        {
                            LogException(task.Exception.InnerException ?? task.Exception, targetMethod);
                        }
                        else
                        {
                            object taskResult = null;
                            if (task.GetType().GetTypeInfo().IsGenericType &&
                                  task.GetType().GetGenericTypeDefinition() == typeof(Task<>))
                            {
                                var property = task.GetType().GetTypeInfo().GetProperties().FirstOrDefault(p => p.Name == "Result");
                                if (property != null)
                                {
                                    taskResult = property.GetValue(task);
                                }
                            }
                            if (_afterAction != null)
                            {
                                this._afterAction(targetMethod, args);
                            }
                        }
                    });
                }
                else
                {
                    try
                    {
                        // _afterAction 事件
                        if (_afterAction != null)
                        {
                            this._afterAction(targetMethod, args);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Do not stop method execution if exception  
                        LogException(ex);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    LogException(ex.InnerException ?? ex, targetMethod);
                    throw ex.InnerException ?? ex;
                }
                else
                {
                    throw ex;
                }
            }

        }


        /// <summary>
        /// aop异常的处理
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="methodInfo"></param>
        private void LogException(Exception exception, MethodInfo methodInfo = null)
        {
            try
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine($"Class {decorated.GetType().FullName}");
                errorMessage.AppendLine($"Method {methodInfo?.Name} threw exception");
                errorMessage.AppendLine(exception.Message);

                //_logError?.Invoke(errorMessage.ToString());  记录到文件系统
            }
            catch (Exception)
            {
                // ignored  
                //Method should return original exception  

            }
        }
    }
}
