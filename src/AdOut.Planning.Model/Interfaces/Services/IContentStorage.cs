using System.IO;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Services
{
    public interface IContentStorage
    {
        Task CreateAsync(Stream content, string filePath);
        Task DeleteAsync(string filePath);
        Task<Stream> GetAsync(string filePath);
    }
}
