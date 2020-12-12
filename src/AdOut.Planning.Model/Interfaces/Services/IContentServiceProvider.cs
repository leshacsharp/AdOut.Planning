namespace AdOut.Planning.Model.Interfaces.Services
{
    public interface IContentServiceProvider
    {
        IContentService CreateContentService(string contentExtension);
    }
}
