using AdOut.Planning.Core.Content.Helpers;
using AdOut.Planning.Core.Content.Validators.Image;
using AdOut.Planning.Model.Interfaces.Content;
using AdOut.Planning.Model.Interfaces.Repositories;

namespace AdOut.Planning.Core.Content.Factories
{
    public class PngContentFactory : IContentFactory
    {
        private readonly IConfigurationRepository _configurationRepository;
        public PngContentFactory(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public IContentValidator CreateContentValidator()
        {
            return new PngValidator(_configurationRepository);
        }

        public IContentHelper CreateContentHelper()
        {
            return new ImageHelper();
        }
    }
}
