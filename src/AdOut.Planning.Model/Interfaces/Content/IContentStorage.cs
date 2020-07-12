using System.IO;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Content
{
    public interface IContentStorage
    {
        Task CreateObjectAsync(Stream content, string filePath);
        Task DeleteObjectAsync(string filePath);
        Task<Stream> GetObjectAsync(string filePath);

        //todo: maybe need to make 'private' method
        string GenerateFilePath(string filePath);
    }
}
