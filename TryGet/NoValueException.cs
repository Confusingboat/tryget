using System;
using System.Collections.Generic;
using System.Text;

namespace TryGet
{
    public class NoValueException : TryGetException
    {
        public NoValueException() : base("The result has no value.") { }
    }
}
