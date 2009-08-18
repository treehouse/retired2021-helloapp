using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Hello.Utils
{
    [Serializable]
    public class HelloException : Exception
    {
        public HelloException()
        {
        }

        public HelloException(string message)
            : base(message)
        {
        }

        public HelloException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected HelloException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
