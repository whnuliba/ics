using NE.ICS.ORM.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.ORM.Xml
{
    public class BoundSqlAnalyzer
    {
        public BoundSql BoundSql { set; get; }
        public BoundSqlAnalyzer(BoundSql BoundSql)
        {
            this.BoundSql = BoundSql;
        }
        public BoundSqlAnalyzer AnalyzerSql(object var) {
            //获取到条件
            var con = this.BoundSql.Condition.Properties;

            var v = con.GetEnumerator();
            StringBuilder whereiftest = new StringBuilder();
            while (v.MoveNext()) {
                if (Test(ReflectionUtil.GetFieldValue(v.Current.Key, var), v.Current.Value.Right, v.Current.Value.Operator)) {
                    whereiftest.Append($" {v.Current.Value.Content}");
                }
            }
            var setCon = this.BoundSql.Set.Properties;
            var vs = setCon.GetEnumerator();
            StringBuilder setiftest = new StringBuilder();
            while (vs.MoveNext())
            {
                if (Test(ReflectionUtil.GetFieldValue(vs.Current.Key, var), vs.Current.Value.Right, vs.Current.Value.Operator))
                {
                    setiftest.Append($" {vs.Current.Value.Content}");
                }
            }
            StringBuilder where = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(this.BoundSql.Condition.Content))
            {
                where.Append($"{XmlSqlEnum.where.ToString()} {this.BoundSql.Condition.Content}");
                where.Append(whereiftest);
            }
            else {
                if (!string.IsNullOrWhiteSpace(whereiftest.ToString())) {
                    string _iftest = whereiftest.ToString().Trim();
                    where.Append($"{XmlSqlEnum.where.ToString()} {_iftest.Substring(3)}");
                }
            }
            StringBuilder set = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(this.BoundSql.Set.Content))
            {
                set.Append($"{XmlSqlEnum.set.ToString()} {this.BoundSql.Set.Content}");
                set.Append(setiftest);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(setiftest.ToString()))
                {
                    string _iftest = setiftest.ToString().Trim();
                    set.Append($"{XmlSqlEnum.set.ToString()} {_iftest.Substring(1)} ");
                }
            }
            string sqlStr = this.BoundSql.Sql.Replace("#{{set}}",set.ToString())
                                             .Replace("#{{where}}", where.ToString());
            this.BoundSql.ActualSql = sqlStr;//$"{this.BoundSql.Sql} {set}{where}";
            this.BoundSql.ActualParameters = var;
            return this;
        }
        private bool Test(object left, string right, string oper) {
            if (left == null)
                return false;
            switch (oper) {
                case ">":
                    return (int)left > Convert.ToInt32(right);
                case "<": 
                    return (int)left < Convert.ToInt32(right);
                case ">=":
                    return (int)left >= Convert.ToInt32(right);
                case "<=":
                    return (int)left<= Convert.ToInt32(right);
                case "!=":
                    return !left.Equals(GetRightObject(right));
                case "==":
                    return left.Equals(GetRightObject(right));
            }
            return false;
        }
        private object GetRightObject(string right) {
            switch (right) {
                case "null":
                    return null;
            }
            return right;
        }
    }


}
