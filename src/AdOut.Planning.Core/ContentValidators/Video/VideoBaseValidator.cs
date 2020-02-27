using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Validators;
using System;
using System.IO;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ContentValidators.Video
{
    public abstract class VideoBaseValidator : IContentValidator
    {
        public async Task<ContentValidationResult> ValidAsync(Stream content)
        {
            if (content == null)
                throw new ArgumentNullException();

            var isCorrectFormatTask = IsCorrectFormatAsync(content);
            var isCorrectSizeTask = IsCorrectSizeAsync(content);
            var isCorrectDurationTask = IsCorrectDurationAsync(content);

            await Task.WhenAll(isCorrectFormatTask, isCorrectSizeTask, isCorrectDurationTask);

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

            if (!isCorrectSizeTask.Result)
            {
                var sizeError = new ContentError()
                {
                    Code = ContentErrorCode.Size,
                    Description = ValidationMessages.NotCorrectSize
                };

                validationResult.Errors.Add(sizeError);
            }

            if (!isCorrectDurationTask.Result)
            {
                var durationError = new ContentError()
                {
                    Code = ContentErrorCode.Duration,
                    Description = ValidationMessages.NotCorrectDuration
                };

                validationResult.Errors.Add(durationError);
            }

            return validationResult;
        }

        protected abstract Task<bool> IsCorrectFormatAsync(Stream content);
        protected abstract Task<bool> IsCorrectSizeAsync(Stream content);
        protected abstract Task<bool> IsCorrectDurationAsync(Stream content);
    }
}
