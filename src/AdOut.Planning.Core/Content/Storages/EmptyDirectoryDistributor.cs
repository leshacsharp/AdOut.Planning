using AdOut.Planning.Model.Interfaces.Content;

namespace AdOut.Planning.Core.Content.Storages
{
    public class EmptyDirectoryDistributor : IDirectoryDistributor
    {
        public string GetDirectory()
        {
            return string.Empty;
        }
    }
}
