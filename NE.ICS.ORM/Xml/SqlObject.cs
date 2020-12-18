using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.ORM.Xml
{
    public class SqlObject
    {
        public string NameSpace { set; get; }
        public Dictionary<string, BoundSql> BoundSql { set; get; } = new Dictionary<string, BoundSql>();
    }
}
