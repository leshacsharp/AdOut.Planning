namespace AdOut.Planning.Model.Interfaces.Content
{
    public interface IContentHelperProvider
    {
        IContentHelper CreateContentHelper(string contentExtension);
    }
}
