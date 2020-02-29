﻿using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Validators;
using System;
using System.IO;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ContentValidators.Video
{
    public abstract class VideoTemplateValidator : IContentValidator
    {
        public async Task<ContentValidationResult> ValidAsync(Stream content)
        {
            if (content == null)
                throw new ArgumentNullException();

            var isCorrectFormat = await IsCorrectFormatAsync(content);
            if (!isCorrectFormat)
                throw new ArgumentException(ValidationMessages.NotCorrectFormat, nameof(content));

            var validationResult = new ContentValidationResult();
 
            var isCorrectDimensionTask = IsCorrectDimensionAsync(content);
            var isCorrectSizeTask = IsCorrectSizeAsync(content);
            var isCorrectDurationTask = IsCorrectDurationAsync(content);

            await Task.WhenAll(isCorrectDimensionTask, isCorrectSizeTask, isCorrectDurationTask);

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
        protected abstract Task<bool> IsCorrectDimensionAsync(Stream content);
        protected abstract Task<bool> IsCorrectSizeAsync(Stream content);
        protected abstract Task<bool> IsCorrectDurationAsync(Stream content); 
    }
}