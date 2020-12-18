using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.Domain
{
    public class IcsRole:BaseDomain
    {
        public string RoleName { set; get; }
        public string RoleCode { set; get; }
        public string RoleDesc { set; get; }
    }
}
