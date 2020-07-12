using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Content;
using AdOut.Planning.Model.Interfaces.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Content.Validators.Image
{
    public abstract class ImageBaseValidator : IContentValidator
    {
        private readonly IConfigurationRepository _configurationRepository;
        public ImageBaseValidator(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public async Task<ValidationResult<ContentError>> ValidateAsync(Stream content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            //todo: is exception and await needed in this place?
            var isCorrectFormat = await IsCorrectFormatAsync(content);
            if (!isCorrectFormat)
            {
                throw new ArgumentException(ValidationMessages.Content.NotCorrectFormat, nameof(content));
            }

            var validationResult = new ValidationResult<ContentError>();

            var isCorrectDimensionTask = IsCorrectDimensionAsync(content);
            var isCorrectSizeTask = IsCorrectSizeAsync(content);

            await Task.WhenAll(isCorrectDimensionTask, isCorrectSizeTask);

            if (!isCorrectDimensionTask.Result)
            {
                //todo: Is 'Code' needed?
                var dimensionError = new ContentError()
                {
                    Code = ContentErrorCode.Dimension,
                    Description = ValidationMessages.Content.NotCorrectDimension
                };

                validationResult.Errors.Add(dimensionError);
            }
     
            if (!isCorrectSizeTask.Result)
            {
                //todo: Is 'Code' needed?
                var sizeError = new ContentError()
                {
                    Code = ContentErrorCode.Size,
                    Description = ValidationMessages.Content.NotCorrectSize
                };

                validationResult.Errors.Add(sizeError);
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

            using (var image = System.Drawing.Image.FromStream(content))
            {
                return image.Width >= minImageWidth && image.Height >= minImageHeight;
            }
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
