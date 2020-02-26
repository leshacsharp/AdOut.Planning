using AdOut.Planning.Model.Classes;
using System.IO;

namespace AdOut.Planning.Model.Interfaces.Validators
{
    public interface IContentValidator
    {
        ContentValidationResult Valid(Stream content);
    }
}
