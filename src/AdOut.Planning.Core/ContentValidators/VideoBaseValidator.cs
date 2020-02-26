using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Validators;
using System.IO;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ContentValidators
{
    public abstract class VideoBaseValidator : IContentValidator
    {
        public ContentValidationResult Valid(Stream content)
        {
            var isCorrectFormat = IsCorrectFormat(content);
            var isCorrectSize = IsCorrectSize(content);
            var isCorrectDuration = IsCorrectDuration(content);

            var validationResult = new ContentValidationResult();
            if (!isCorrectFormat)
            {
                var formatError = new ContentError()
                {
                    Code = ContentErrorCode.Format,
                    Description = ValidationMessages.NotCorrectFormat
                };

                validationResult.Errors.Add(formatError);
            }

            if (!isCorrectSize)
            {
                var sizeError = new ContentError()
                {
                    Code = ContentErrorCode.Size,
                    Description = ValidationMessages.NotCorrectSize
                };

                validationResult.Errors.Add(sizeError);
            }

            if (!isCorrectDuration)
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

        protected abstract bool IsCorrectFormat(Stream content);
        protected abstract bool IsCorrectSize(Stream content);
        protected abstract bool IsCorrectDuration(Stream content);
    }
}
