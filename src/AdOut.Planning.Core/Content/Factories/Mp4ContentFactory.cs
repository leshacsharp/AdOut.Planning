using AdOut.Planning.Core.Content.Helpers;
using AdOut.Planning.Core.Content.Validators.Video;
using AdOut.Planning.Model.Interfaces.Content;
using AdOut.Planning.Model.Interfaces.Repositories;

namespace AdOut.Planning.Core.Content.Factories
{
    public class Mp4ContentFactory : IContentFactory
    {
        private readonly IConfigurationRepository _configurationRepository;
        public Mp4ContentFactory(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public IContentValidator CreateContentValidator()
        {
            return new Mp4Validator(_configurationRepository);
        }

        public IContentHelper CreateContentHelper()
        {
            return new VideoHelper();
        }
    }
}
