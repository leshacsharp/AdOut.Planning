using AdOut.Planning.Model.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.ContentValidators.Video
{
    public abstract class FFmpegBaseValidator : VideoTemplateValidator
    {
        private readonly IConfigurationRepository _configurationRepository;

        protected override Task<bool> IsCorrectDimensionAsync(Stream content)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> IsCorrectSizeAsync(Stream content)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> IsCorrectDurationAsync(Stream content)
        {
            throw new NotImplementedException();
        }
    }
}
