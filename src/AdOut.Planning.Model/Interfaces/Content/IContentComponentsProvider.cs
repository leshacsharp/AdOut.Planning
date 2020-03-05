namespace AdOut.Planning.Model.Interfaces.Content
{
    public interface IContentComponentsProvider
    {
        IContentFactory CreateContentFactory(string contentExtension);
    }
}
