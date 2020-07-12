namespace AdOut.Planning.Model.Interfaces.Content
{
    ///todo: maybe need to make a abstract factory with IContentHelperProvider !!!
    public interface IContentValidatorProvider
    {
        IContentValidator CreateContentValidator(string contentExtension);
    }
}
