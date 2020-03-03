using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ContentValidators.Image
{
    public abstract class ImageBaseValidator : ImageTemplateValidator
    {
        private readonly IConfigurationRepository _configurationRepository;

        public ImageBaseValidator(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        protected override async Task<bool> IsCorrectDimensionAsync(Stream content)
        {
            var minImageDimensionConfig = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MinImageDimension);
            var dimensionParts = minImageDimensionConfig.Split('x', StringSplitOptions.RemoveEmptyEntries);

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
            var maxImageSizeConfig = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MaxImageSize);
            var maxImageSizeMb  = int.Parse(maxImageSizeConfig);

            var imageSizeMb = content.Length / ContentSizes.Mb;
            return imageSizeMb <= maxImageSizeMb;
        }
    }
}
