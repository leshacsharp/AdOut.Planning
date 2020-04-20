using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Exceptions;
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
                var dimensionError = new ContentError()
                {
                    Code = ContentErrorCode.Dimension,
                    Description = ValidationMessages.Content.NotCorrectDimension
                };

                validationResult.Errors.Add(dimensionError);
            }

            if (!isCorrectSizeTask.Result)
            {
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
            var minImageDimensionConfig = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MinImageDimension);
            var dimensionParts = minImageDimensionConfig.Split('x', StringSplitOptions.RemoveEmptyEntries);

            if (dimensionParts.Length != 2)
            {
                throw new ConfigurationException("Invalid image dimesion config");
            }

            var minImageWidth = int.Parse(dimensionParts[0]);
            var minImageHeight = int.Parse(dimensionParts[1]);

            using (var image = System.Drawing.Image.FromStream(content))
            {
                return image.Width >= minImageWidth && image.Height >= minImageHeight;
            }
        }

        protected virtual async Task<bool> IsCorrectSizeAsync(Stream content)
        {
            var maxImageSizeConfig = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MaxImageSize);
            var maxImageSizeMb = int.Parse(maxImageSizeConfig);

            var imageSizeMb = content.Length / ContentSizes.Mb;
            return imageSizeMb <= maxImageSizeMb;
        }
    }
}
