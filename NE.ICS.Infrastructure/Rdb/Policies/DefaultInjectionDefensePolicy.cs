using System.Collections;

namespace NE.ICS.Infrastructure.Policies
{
    // SQL 注入终极解决方案：使用参数化语句或参数化存储过程。
    // 采用正则表达式进行带注入特征的语句检测过滤(比较勉强)。
    public class DefaultInjectionDefensePolicy : IInjectionDefensePolicy
    {
        private readonly SqlRegex _sqlRegex;
        public DefaultInjectionDefensePolicy(SqlRegex sqlRegex = null)
        {            
            _sqlRegex = sqlRegex ?? SqlRegex.Default;
        }

        public bool DetectInjectionWithParameters { get; set;}

        public bool DetectInjection(string sql, IEnumerable parameters = null)
        {
            // sql 检测未实现
            return DetectParameters(parameters);
        }

        // 默认实现支持对where Clause 或 orderBy Clause 进行注入检查
        public bool DetectInjection(string whereClause, string orderBy, IEnumerable parameters = null)
        {
            if (_sqlRegex.DetectSqlInjection(whereClause, orderBy))
            {
                return true;
            }
            return DetectParameters(parameters);
        }

        private bool DetectParameters(IEnumerable parameters)
        {
            if (DetectInjectionWithParameters && parameters != null)
            {
                foreach (var param in parameters)
                {
                    var s = param as string;
                    if (s != null)
                    {
                        if (_sqlRegex.DetectSqlInjection(s))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


    }
}
