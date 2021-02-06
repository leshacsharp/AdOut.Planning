using AdOut.Planning.Core.Validators.Base;
using AdOut.Planning.Model;
using AdOut.Planning.Model.Interfaces.Repositories;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Validators.Content
{
    public class PngValidator : ImageBaseValidator
    {
        public PngValidator(IConfigurationRepository configurationRepository)
            : base(configurationRepository)
        {
        }

        protected override async Task<bool> IsCorrectFormatAsync(Stream content)
        {
            var signature = Constants.ContentSignatures.PNG;
            if (content.Length < signature.Length)
            {
                return false;
            }

            var buffer = new byte[signature.Length];
            await content.ReadAsync(buffer, 0, buffer.Length);

            return buffer.SequenceEqual(signature);
        }
    }
}
