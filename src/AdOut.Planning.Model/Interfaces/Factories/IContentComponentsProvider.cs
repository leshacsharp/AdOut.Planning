namespace AdOut.Planning.Model.Interfaces.Factories
{
    public interface IContentComponentsProvider
    {
        IContentFactory CreateContentFactory(string contentExtension);
    }
}
