using System;
namespace Covarity.Exceptions
{
    public class InvalidTrayItemException : Exception
    {
        public InvalidTrayItemException(string msg) : base(msg) { }
    }
}
