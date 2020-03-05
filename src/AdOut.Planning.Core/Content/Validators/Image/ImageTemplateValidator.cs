using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Content;
using System;
using System.IO;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Content.Validators.Image
{
    public abstract class ImageTemplateValidator : IContentValidator
    {
        public async Task<ContentValidationResult> ValidAsync(Stream content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var isCorrectFormat = await IsCorrectFormatAsync(content);
            if (!isCorrectFormat)
                throw new ArgumentException(ValidationMessages.NotCorrectFormat, nameof(content));

            var validationResult = new ContentValidationResult();

            var isCorrectDimensionTask = IsCorrectDimensionAsync(content);
            var isCorrectSizeTask = IsCorrectSizeAsync(content);

            await Task.WhenAll(isCorrectDimensionTask, isCorrectSizeTask);

            if (!isCorrectDimensionTask.Result)
            {
                var dimensionError = new ContentError()
                {
                    Code = ContentErrorCode.Dimension,
                    Description = ValidationMessages.NotCorrectDimension
                };

                validationResult.Errors.Add(dimensionError);
            }

            if (!isCorrectSizeTask.Result)
            {
                var sizeError = new ContentError()
                {
                    Code = ContentErrorCode.Size,
                    Description = ValidationMessages.NotCorrectSize
                };

                validationResult.Errors.Add(sizeError);
            }

            return validationResult;
        }

        protected abstract Task<bool> IsCorrectFormatAsync(Stream content);
        protected abstract Task<bool> IsCorrectDimensionAsync(Stream content);
        protected abstract Task<bool> IsCorrectSizeAsync(Stream content);
    }
}
