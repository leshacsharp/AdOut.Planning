using AdOut.Planning.Model;
using AdOut.Planning.Model.Interfaces.Repositories;

namespace AdOut.Planning.Core.ContentValidators.Image
{
    public class PNGValidator : ImageBaseValidator
    {
        public PNGValidator(IConfigurationRepository configurationRepository)
            : base(configurationRepository, Constants.ContentSignature.PNG)
        {
        }
    }
}
