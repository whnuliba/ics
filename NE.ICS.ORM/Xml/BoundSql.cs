using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.ORM.Xml
{
   public class BoundSql
    {
        public XmlSqlEnum Command { set; get; }
        public string ResultType { set; get; }
        public string ParameterType { set; get; }
        public string Id { set; get; }
        public string Sql { set; get; }
        public ExtCondition Condition { set; get; } = new ExtCondition();
        public string ActualSql { set; get; }
        public object ActualParameters { set; get; }
        public ExtCondition Set { set; get; } = new ExtCondition();
    }
}
