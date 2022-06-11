using System;
using System.Collections.Generic;
using System.Text;

namespace TryGet
{
    public abstract class TryGetException : Exception
    {
        protected TryGetException(string message) : base(message) { }
    }
}
