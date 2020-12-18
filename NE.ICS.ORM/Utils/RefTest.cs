using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.ORM.Utils
{
    public class RefTest
    {
        public string Id;
        public InnerTest Test;
    }
    public class InnerTest {
        public string name;
        public int age;
    }
}
