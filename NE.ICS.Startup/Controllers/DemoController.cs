using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using NE.ICS.ORM.proxy;
using NE.ICS.ORM.Utils;
using NE.ICS.ORM.Xml;
using NE.ICS.Domain;
using NE.ICS.Infrastructure;
using NE.ICS.OAuth.Mapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NE.ICS.RestStartup.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController
    {
        [HttpGet]
        [Route("test")]
        public object getTest() {
            IcsUser iUser1 = new IcsUser()
            {
            RealName="蔡清"
            };
            //执行代理
            var serviceProxy = new MethodProxy<IcsUserMapper>();
            IcsUserMapper user = serviceProxy.Create();  //
            object d =user.SelectUser(iUser1);
            return d;
        }
    }
}
