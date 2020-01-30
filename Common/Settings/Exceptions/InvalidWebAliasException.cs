using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Exceptions
{
    public class InvalidWebAliasException : Exception
    {
        public InvalidWebAliasException()
        {

        }
        public InvalidWebAliasException(string message)
            : base(message)
        {
        }

        public InvalidWebAliasException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}