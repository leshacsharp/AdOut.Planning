using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ContentValidators.Image
{
    public abstract class ImageBaseValidator : ImageTemplateValidator
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly byte[] _imageSignature;

        public ImageBaseValidator(IConfigurationRepository configurationRepository, byte[] imageSignature)
        {
            _configurationRepository = configurationRepository;
            _imageSignature = imageSignature;
        }

        protected override async Task<bool> IsCorrectFormatAsync(Stream content)
        {     
            if (content.Length < _imageSignature.Length)
            {
                return false;
            }

            var buffer = new byte[_imageSignature.Length];
            await content.ReadAsync(buffer, 0, buffer.Length);

            return buffer.SequenceEqual(_imageSignature);
        }

        protected override async Task<bool> IsCorrectDimensionAsync(Stream content)
        {
            var imageDimensionConfig = await _configurationRepository.Read(c => c.Type == ConfigurationsTypes.ImageDimension).SingleAsync();
            var dimensionParts = imageDimensionConfig.Value.Split('x', StringSplitOptions.RemoveEmptyEntries);

            if (dimensionParts.Length != 2)
                throw new ConfigurationException("Invalid image dimesion config");

            var minImageWidth = int.Parse(dimensionParts[0]);
            var minImageHeight = int.Parse(dimensionParts[1]);

            using (var image = System.Drawing.Image.FromStream(content))
            {
                return image.Width >= minImageWidth && image.Height >= minImageHeight;  
            }
        }

        protected override async Task<bool> IsCorrectSizeAsync(Stream content)
        {
            var imageSizeConfig = await _configurationRepository.Read(c => c.Type == ConfigurationsTypes.ImageSize).SingleAsync();
            var maxImageSize  = int.Parse(imageSizeConfig.Value);

            if(content.Length != 0)
            {
                return false;
            }

            var imageSizeKb = content.Length / ContentSizes.Kb;
            return imageSizeKb <= maxImageSize;
        }
    }
}
