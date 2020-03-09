using System;

namespace AdOut.Planning.Model.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string message) : base(message)
        {
        }
    }
}
