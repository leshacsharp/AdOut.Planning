using AdOut.Planning.Model.Interfaces.Factories;
using AdOut.Planning.Model.Interfaces.Repositories;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Content.Factories
{
    public class ContentComponentsProvider : IContentComponentsProvider
    {
        private readonly IConfigurationRepository _configurationRepository;
        public ContentComponentsProvider(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public IContentFactory CreateContentFactory(string contentExtension)
        {
            if (contentExtension == null)
                throw new ArgumentNullException(nameof(contentExtension));

            return contentExtension switch
            {
                var ext when ext is ContentExtensions.JPEG || ext is ContentExtensions.JPG => new JpegContentFactory(_configurationRepository),
                ContentExtensions.PNG => new PngContentFactory(_configurationRepository),
                ContentExtensions.AVI => new AviContentFactory(_configurationRepository),
                ContentExtensions.MP4 => new Mp4ContentFactory(_configurationRepository),
                _ => throw new NotSupportedException($"Extension={contentExtension} is not supported")
            };
        }
    }
}
