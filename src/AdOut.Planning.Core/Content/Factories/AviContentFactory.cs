using AdOut.Planning.Core.Content.Helpers;
using AdOut.Planning.Core.Content.Validators.Video;
using AdOut.Planning.Model.Interfaces.Factories;
using AdOut.Planning.Model.Interfaces.Helpers;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Validators;

namespace AdOut.Planning.Core.Content.Factories
{
    public class AviContentFactory : IContentFactory
    {
        private readonly IConfigurationRepository _configurationRepository;
        public AviContentFactory(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public IContentValidator CreateContentValidator()
        {
            return new AviValidator(_configurationRepository);
        }

        public IContentHelper CreateContentHelper()
        {
            return new VideoHelper();
        }
    }
}
