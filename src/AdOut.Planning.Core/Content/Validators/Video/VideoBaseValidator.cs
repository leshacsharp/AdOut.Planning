using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Content;
using AdOut.Planning.Model.Interfaces.Repositories;
using Alturos.VideoInfo;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Content.Validators.Video
{
    public abstract class VideoBaseValidator : IContentValidator
    {
        private readonly IConfigurationRepository _configurationRepository;
        public VideoBaseValidator(IConfigurationRepository configurationRepository)
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
                throw new ArgumentException(ContentValidationMessages.NotCorrectFormat, nameof(content));
            }

            var validationResult = new ValidationResult<ContentError>();
 
            var isCorrectDimensionTask = IsCorrectDimensionAsync(content);
            var isCorrectSizeTask = IsCorrectSizeAsync(content);
            var isCorrectDurationTask = IsCorrectDurationAsync(content);

            await Task.WhenAll(isCorrectDimensionTask, isCorrectSizeTask, isCorrectDurationTask);

            if (!isCorrectDimensionTask.Result)
            {
                var dimensionError = new ContentError()
                {
                    Code = ContentErrorCode.Dimension,
                    Description = ContentValidationMessages.NotCorrectDimension
                };

                validationResult.Errors.Add(dimensionError);
            }

            if (!isCorrectSizeTask.Result)
            {
                var sizeError = new ContentError()
                {
                    Code = ContentErrorCode.Size,
                    Description = ContentValidationMessages.NotCorrectSize
                };

                validationResult.Errors.Add(sizeError);
            }

            if (!isCorrectDurationTask.Result)
            {
                var durationError = new ContentError()
                {
                    Code = ContentErrorCode.Duration,
                    Description = ContentValidationMessages.NotCorrectDuration
                };

                validationResult.Errors.Add(durationError);
            }

            return validationResult;
        }

        protected abstract Task<bool> IsCorrectFormatAsync(Stream content);

        protected virtual async Task<bool> IsCorrectDimensionAsync(Stream content)
        {
            var minVideoDimensionConfig = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MinVideoDimension);
            var dimensionParts = minVideoDimensionConfig.Split('x', StringSplitOptions.RemoveEmptyEntries);

            if (dimensionParts.Length != 2)
            {
                throw new ConfigurationException("Invalid video dimesion config");
            }

            var minVideoWidth = int.Parse(dimensionParts[0]);
            var minVideoHeight = int.Parse(dimensionParts[1]);

            var videoInfo = await GetVideoInfoAsync(content);
            return videoInfo.Width >= minVideoWidth && videoInfo.Height >= minVideoHeight;
        }

        protected virtual async Task<bool> IsCorrectSizeAsync(Stream content)
        {
            var maxVideoSizeConfig = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MaxVideoSize);
            var maxVideoSizeMb = int.Parse(maxVideoSizeConfig);

            var imageSizeMb = content.Length / ContentSizes.Mb;
            return imageSizeMb <= maxVideoSizeMb;
        }

        protected virtual async Task<bool> IsCorrectDurationAsync(Stream content)
        {
            var minVideoDurationMinConfigTask = _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MinVideoDuration);
            var maxVideoDurationConfigTask = _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MaxVideoDuration);

            await Task.WhenAll(minVideoDurationMinConfigTask, maxVideoDurationConfigTask);

            var minVideoDurationSec = int.Parse(minVideoDurationMinConfigTask.Result);
            var maxVideoDurationSec = int.Parse(maxVideoDurationConfigTask.Result);

            var videoInfo = await GetVideoInfoAsync(content);
            var videoDurationSec = videoInfo.Duration;

            return videoDurationSec >= minVideoDurationSec && videoDurationSec <= maxVideoDurationSec;
        }

        private async Task<Alturos.VideoInfo.Model.Stream> GetVideoInfoAsync(Stream content)
        {
            var videoBuffer = new byte[content.Length];
            await content.ReadAsync(videoBuffer, 0, videoBuffer.Length);

            var videoAnalyzer = new VideoAnalyzer();
            var analyzerResult = await videoAnalyzer.GetVideoInfoAsync(videoBuffer);

            var videoStream = analyzerResult.VideoInfo.Streams.Single(s => s.CodecType == CodecTypes.Video);
            return videoStream;
        }
    }
}
