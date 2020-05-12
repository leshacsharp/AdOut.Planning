using System;

namespace AdOut.Planning.Model.Exceptions
{
    public class ForbiddenException : Exception, IHttpException
    {
        public ForbiddenException(string message = null) : base(message)
        {
        }

        public int HttpStatusCode => Constants.HttpStatusCodes.Status403Forbidden;
    }
}
