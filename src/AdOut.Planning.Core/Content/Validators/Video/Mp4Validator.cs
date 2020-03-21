using AdOut.Planning.Model;
using AdOut.Planning.Model.Interfaces.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Content.Validators.Video
{
    public class Mp4Validator : VideoBaseValidator
    {
        public Mp4Validator(IConfigurationRepository configurationRepository) : 
            base(configurationRepository)
        {
        }

        protected override async Task<bool> IsCorrectFormatAsync(Stream content)
        {
            var signature = Constants.ContentSignatures.MP4;
            if(content.Length < signature.Length)
            {
                return false;
            }

            var buffer = new byte[signature.Length];
            await content.ReadAsync(buffer, 0, buffer.Length);

            return buffer.SequenceEqual(signature);
        }
    }
}
