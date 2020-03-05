using System.IO;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Content
{
    public interface IContentStorage
    {
        Task CreateFileAsync(Stream content, string filePath);
        void DeleteFile(string filePath);
        string GenerateFilePath(string fileExtension);
    }
}
