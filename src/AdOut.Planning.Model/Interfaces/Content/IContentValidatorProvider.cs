namespace AdOut.Planning.Model.Interfaces.Content
{
    public interface IContentValidatorProvider
    {
        IContentValidator CreateContentValidator(string contentExtension);
    }
}
