using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Validators.Base
{
    public abstract class ImageBaseValidator : IContentValidator
    {
        private readonly IConfigurationRepository _configurationRepository;
        public ImageBaseValidator(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public async Task<ValidationResult<string>> ValidateAsync(Stream content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var validationResult = new ValidationResult<string>();
            var isCorrectFormat = await IsCorrectFormatAsync(content);

            if (!isCorrectFormat)
            {
                validationResult.Errors.Add(ValidationMessages.Content.InvalidFormat);
                return validationResult;
            }

            var isCorrectDimension = await IsCorrectDimensionAsync(content);
            var isCorrectSize = await IsCorrectSizeAsync(content);

            if (!isCorrectDimension)
            {
                validationResult.Errors.Add(ValidationMessages.Content.NotCorrectDimension);
            }
            if (!isCorrectSize)
            {
                validationResult.Errors.Add(ValidationMessages.Content.NotCorrectSize);
            }

            return validationResult;
        }

        protected abstract Task<bool> IsCorrectFormatAsync(Stream content);

        protected virtual async Task<bool> IsCorrectDimensionAsync(Stream content)
        {
            var minImageWidthCfg = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MinImageWidth);
            var minImageHeightCfg = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MinImageHeight);
            var minImageWidth = int.Parse(minImageWidthCfg);
            var minImageHeight = int.Parse(minImageHeightCfg);

            using var image = System.Drawing.Image.FromStream(content);
            return image.Width >= minImageWidth && image.Height >= minImageHeight;
        }

        protected virtual async Task<bool> IsCorrectSizeAsync(Stream content)
        {
            var maxImageSizeCfg = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MaxImageSizeKb);
            var maxImageSizeKb = int.Parse(maxImageSizeCfg);

            var imageSizeKb = content.Length / ContentSizes.Kb;
            return imageSizeKb <= maxImageSizeKb;
        }
    }
}
