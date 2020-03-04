using AdOut.Planning.Core.Content.Helpers;
using AdOut.Planning.Core.Content.Validators.Image;
using AdOut.Planning.Model.Interfaces.Factories;
using AdOut.Planning.Model.Interfaces.Helpers;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Validators;

namespace AdOut.Planning.Core.Content.Factories
{
    public class JpegContentFactory : IContentFactory
    {
        private readonly IConfigurationRepository _configurationRepository;
        public JpegContentFactory(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public IContentValidator CreateContentValidator()
        {
            return new JpegValidator(_configurationRepository);
        }

        public IContentHelper CreateContentHelper()
        {
            return new ImageHelper();
        }
    }
}
