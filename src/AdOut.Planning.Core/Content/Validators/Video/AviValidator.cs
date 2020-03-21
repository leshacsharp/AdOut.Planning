using AdOut.Planning.Model;
using AdOut.Planning.Model.Interfaces.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Content.Validators.Video
{
    public class AviValidator : VideoBaseValidator
    {
        public AviValidator(IConfigurationRepository configurationRepository)
            : base(configurationRepository)
        {
        }

        protected override async Task<bool> IsCorrectFormatAsync(Stream content)
        {
            var signatures = Constants.ContentSignatures.AVI;
            var neededLengthOfStream = signatures.Max(s => s.Length);

            if (content.Length < neededLengthOfStream)
            {
                return false;
            }

            var buffer = new byte[neededLengthOfStream];
            await content.ReadAsync(buffer, 0, buffer.Length);

            var haveOneOfTheSignatures = signatures.Any(signature => 
            {
                var bufferWithSignatureLength = new byte[signature.Length];
                Array.Copy(buffer, 0, bufferWithSignatureLength, 0, bufferWithSignatureLength.Length);

                return bufferWithSignatureLength.SequenceEqual(signature);
            });

            return haveOneOfTheSignatures;
        }
    }
}
