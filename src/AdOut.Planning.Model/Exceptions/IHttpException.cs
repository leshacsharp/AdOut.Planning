namespace AdOut.Planning.Model.Exceptions
{
    public interface IHttpException
    {
        int HttpStatusCode { get; }
    }
}
