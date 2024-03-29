﻿using AdOut.Planning.Core.Validators.Base;
using AdOut.Planning.Model;
using AdOut.Planning.Model.Interfaces.Repositories;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Validators.Content
{
    public class JpegValidator : ImageBaseValidator
    {
        public JpegValidator(IConfigurationRepository configurationRepository)
            : base(configurationRepository)
        {
        }

        protected override async Task<bool> IsCorrectFormatAsync(Stream content)
        {
            var startSignature = Constants.ContentSignatures.JPEG_START;
            var endSignature = Constants.ContentSignatures.JPEG_END;

            if (content.Length < startSignature.Length + endSignature.Length)
            {
                return false;
            }

            var startBuffer = new byte[startSignature.Length];
            await content.ReadAsync(startBuffer, 0, startBuffer.Length);

            var endBuffer = new byte[endSignature.Length];
            var endBufferOffset = (int)content.Length - endBuffer.Length;
            content.Seek(endBufferOffset, SeekOrigin.Begin);
            await content.ReadAsync(endBuffer, 0, endBuffer.Length);

            return startSignature.SequenceEqual(startBuffer) && endSignature.SequenceEqual(endBuffer);
        }
    }
}
