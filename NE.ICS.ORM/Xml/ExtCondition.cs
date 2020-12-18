using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.ORM.Xml
{
    public class ExtCondition
    {
        public Dictionary<string, IfTest> Properties { set; get; } = new Dictionary<string, IfTest>();
        public string Content { set; get; }
        public ExtCondition Condition { set; get; }


    }

    public class IfTest { 
        public string Left { set; get; }
        public string Operator { set; get; }
        public string Right { set; get; }
        public string Content { set; get; }
    }
}
