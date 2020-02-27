using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Interfaces.Validators;
using AdOut.Planning.Model.Enum;
using System.IO;
using System;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ContentValidators.Image
{
    public abstract class ImageTemplateValidator : IContentValidator
    {
        public async Task<ContentValidationResult> ValidAsync(Stream content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var isCorrectFormatTask = IsCorrectFormatAsync(content);
            var isCorrectSizeTask = IsCorrectSizeAsync(content);

            await Task.WhenAll(isCorrectFormatTask, isCorrectSizeTask);

            var validationResult = new ContentValidationResult();
            if (!isCorrectFormatTask.Result)
            {
                var formatError = new ContentError()
                {
                    Code = ContentErrorCode.Format,
                    Description = ValidationMessages.NotCorrectFormat
                };

                validationResult.Errors.Add(formatError);
            }

            if(!isCorrectSizeTask.Result)
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
