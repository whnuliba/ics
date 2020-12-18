using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.Domain
{
    public class IcsUser: BaseDomain
    {
        public string UserName { set; get; }
        public string RealName { set; get; }
        public string Password { set; get; }
    }
}
