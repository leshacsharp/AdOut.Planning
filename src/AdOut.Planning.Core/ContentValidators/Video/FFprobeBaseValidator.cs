using AdOut.Planning.Model.Interfaces.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.ContentValidators.Video
{
    public abstract class FFprobeBaseValidator : VideoTemplateValidator
    {
        private readonly IConfigurationRepository _configurationRepository;

        public FFprobeBaseValidator(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

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
