using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Services;
using System;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Validators.Content
{
    public class ContentValidatorProvider : IContentValidatorProvider
    {
        private readonly IConfigurationRepository _configurationRepository;
        public ContentValidatorProvider(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public IContentValidator CreateContentValidator(string contentExtension)
        {
            if (contentExtension == null)
            {
                throw new ArgumentNullException(nameof(contentExtension));
            }

            return contentExtension switch
            {
                var ext when ext is ContentExtensions.JPEG || ext is ContentExtensions.JPG => new JpegValidator(_configurationRepository),
                ContentExtensions.PNG => new PngValidator(_configurationRepository),
                ContentExtensions.AVI => new AviValidator(_configurationRepository),
                ContentExtensions.MP4 => new Mp4Validator(_configurationRepository),
                _ => throw new NotSupportedException($"Extension={contentExtension} is not supported")
            };
        }
    }
}
