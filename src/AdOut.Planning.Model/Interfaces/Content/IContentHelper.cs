using System.IO;

namespace AdOut.Planning.Model.Interfaces.Content
{
    public interface IContentHelper
    {
        Stream GetThumbnail(Stream content, int width, int height);
    }
}
