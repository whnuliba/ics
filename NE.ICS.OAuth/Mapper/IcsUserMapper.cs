using NE.ICS.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.OAuth.Mapper
{
    public interface IcsUserMapper
    {
        public List<object> SelectUser(IcsUser user);
    }
}
