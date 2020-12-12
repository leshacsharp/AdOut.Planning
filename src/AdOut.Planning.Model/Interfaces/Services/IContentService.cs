using System.IO;

namespace AdOut.Planning.Model.Interfaces.Services
{
    public interface IContentService
    {
        Stream GetThumbnail(Stream content, int width, int height);
    }
}
