using AdOut.Planning.Model.Interfaces.Helpers;
using AdOut.Planning.Model.Interfaces.Validators;

namespace AdOut.Planning.Model.Interfaces.Factories
{
    public interface IContentFactory
    {
        IContentValidator CreateContentValidator();
        IContentHelper CreateContentHelper();
    }
}
