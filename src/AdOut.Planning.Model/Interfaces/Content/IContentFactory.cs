namespace AdOut.Planning.Model.Interfaces.Content
{
    public interface IContentFactory
    {
        IContentValidator CreateContentValidator();
        IContentHelper CreateContentHelper();
    }
}
