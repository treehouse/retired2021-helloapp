using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Hello.Bot
{
    [Serializable]
    public class HelloAppException : Exception
    {
        public HelloAppException()
        {
        }

        public HelloAppException(string message)
            : base(message)
        {
        }

        public HelloAppException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected HelloAppException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
