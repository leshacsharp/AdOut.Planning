using AdOut.Planning.Model.Classes;
using System.IO;
using System.Threading.Tasks;

namespace AdOut.Planning.Model.Interfaces.Content
{
    public interface IContentValidator
    {
        Task<ValidationResult<ContentError>> ValidateAsync(Stream content);
    }
}
