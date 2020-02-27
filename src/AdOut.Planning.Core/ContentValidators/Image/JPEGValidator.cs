using AdOut.Planning.Model;
using AdOut.Planning.Model.Interfaces.Repositories;

namespace AdOut.Planning.Core.ContentValidators.Image
{
    public class JPEGValidator : ImageBaseValidator
    {
        public JPEGValidator(IConfigurationRepository configurationRepository)
            : base(configurationRepository, Constants.ContentSignature.JPEG)
        {

        }
    }
}
