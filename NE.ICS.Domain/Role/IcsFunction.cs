using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.Domain
{
    public class IcsFunction
    {
        /// <summary>
        /// URL
        /// </summary>
        public string FuncUrl { set; get; }
        /// <summary>
        /// 程序集或者全类名
        /// </summary>
        public string FuncClass { set; get; }
        public string MenuId { set; get; }
        public string RoleId { set; get; }

    }
}
