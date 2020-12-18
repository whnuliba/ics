using System;
using System.Runtime.Serialization;

namespace NE.ICS.Infrastructure
{
    [Serializable]
    public class DbAccessExeception : ApplicationException
    {
        public DbAccessExeception()
            : base("DBAccessExeception")
        {
            
        }

        public DbAccessExeception(string message)
            : base(message)
        {
            
        }

        public DbAccessExeception(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }


        protected DbAccessExeception(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
