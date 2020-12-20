using System;
using System.Collections.Generic;
using System.Text;

namespace NE.ICS.ORM
{
    public class IcsOrmException : Exception
    {
        public IcsOrmException(string message) : base(message)
        {

        }
        public IcsOrmException()
          : base("IcsOrmException")
        {

        }

        public IcsOrmException(string message, Exception inner)//指定错误消息和内部异常信息
            : base(message, inner)
        {


        }
    }
    }
