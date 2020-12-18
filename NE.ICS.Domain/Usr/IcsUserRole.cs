using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.Domain
{
    public class IcsUserRole:BaseDomain
    {
        public string UserId { set; get; }
        public string RoleId { set; get; }
    }
}
