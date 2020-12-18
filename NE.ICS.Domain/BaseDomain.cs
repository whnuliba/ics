using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.Domain
{
    public class BaseDomain
    {
        public string Id { set; get; }
        public string CreateUser { set; get; }
        public string LastModifyUser { set; get; }
        public DateTime? CreateDate { set; get; }
        public DateTime? LastModifyDate { set; get; }
        public int? Status { set; get; }
    }
}
