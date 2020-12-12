namespace AdOut.Planning.Model.Interfaces.Services
{
    public interface IContentValidatorProvider
    {
        IContentValidator CreateContentValidator(string contentExtension);
    }
}
