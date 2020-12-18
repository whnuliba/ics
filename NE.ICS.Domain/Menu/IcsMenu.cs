using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.Domain
{
    public class IcsMenu:BaseDomain
    {
        public string MenuName { set; get; }
        public string EName { set; get; }
        public string MenuUrl { set; get; }
        public string MenuClass { set; get; }
        public string ParentId { set; get; }
        public int Sort { set; get; }
    }
}
