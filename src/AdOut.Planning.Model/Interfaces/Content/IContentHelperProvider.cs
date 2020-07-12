namespace AdOut.Planning.Model.Interfaces.Content
{
    //todo: maybe need to make a abstract factory with IContentValidatorProvider !!!
    public interface IContentHelperProvider
    {
        IContentHelper CreateContentHelper(string contentExtension);
    }
}
