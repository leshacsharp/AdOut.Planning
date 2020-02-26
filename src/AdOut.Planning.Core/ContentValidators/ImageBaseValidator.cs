using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Interfaces.Validators;
using AdOut.Planning.Model.Enum;
using System.IO;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ContentValidators
{
    public abstract class ImageBaseValidator : IContentValidator
    {
        public ContentValidationResult Valid(Stream content)
        {
            var isCorrectFormat = IsCorrectFormat(content);
            var isCorrectSize = IsCorrectSize(content);

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

            if(!isCorrectSize)
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

        protected abstract bool IsCorrectFormat(Stream content);
        protected abstract bool IsCorrectSize(Stream content);
    }
}
