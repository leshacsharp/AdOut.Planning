using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Enum;
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

            //todo: is exception and await needed in this place?
            var isCorrectFormat = await IsCorrectFormatAsync(content);
            if (!isCorrectFormat)
            {
                throw new ArgumentException(ValidationMessages.Content.NotCorrectFormat, nameof(content));
            }

            var validationResult = new ValidationResult<ContentError>();
            
            var isCorrectDimension = await IsCorrectDimensionAsync(content);
            var isCorrectSize = await IsCorrectSizeAsync(content);
            var isCorrectDuration = await IsCorrectDurationAsync(content);

            if (!isCorrectDimension)
            {
                //todo: Is 'Code' needed?
                var dimensionError = new ContentError()
                {
                    Code = ContentErrorCode.Dimension,
                    Description = ValidationMessages.Content.NotCorrectDimension
                };

                validationResult.Errors.Add(dimensionError);
            }

            if (!isCorrectSize)
            {
                //todo: Is 'Code' needed?
                var sizeError = new ContentError()
                {
                    Code = ContentErrorCode.Size,
                    Description = ValidationMessages.Content.NotCorrectSize
                };

                validationResult.Errors.Add(sizeError);
            }

            if (!isCorrectDuration)
            {
                var durationError = new ContentError()
                {
                    Code = ContentErrorCode.Duration,
                    Description = ValidationMessages.Content.NotCorrectDuration
                };

                validationResult.Errors.Add(durationError);
            }

            return validationResult;
        }

        protected abstract Task<bool> IsCorrectFormatAsync(Stream content);

        protected virtual async Task<bool> IsCorrectDimensionAsync(Stream content)
        {
            var minVideoWidthCfg = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MinVideoWidth);
            var minVideoHeightCfg = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MinVideoHeight);

            var minVideoWidth = int.Parse(minVideoWidthCfg);
            var minVideoHeight = int.Parse(minVideoHeightCfg);

            var videoInfo = await GetVideoInfoAsync(content);
            return videoInfo.Width >= minVideoWidth && videoInfo.Height >= minVideoHeight;
        }

        protected virtual async Task<bool> IsCorrectSizeAsync(Stream content)
        {
            var maxVideoSizeCfg = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MaxVideoSizeKb);
            var maxVideoSizeKb = int.Parse(maxVideoSizeCfg);

            var imageSizeKb = content.Length / ContentSizes.Kb;
            return imageSizeKb <= maxVideoSizeKb;
        }

        protected virtual async Task<bool> IsCorrectDurationAsync(Stream content)
        {
            var minVideoDurationCfg = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MinVideoDuration);
            var maxVideoDurationCfg = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MaxVideoDuration);

            var minVideoDurationSec = int.Parse(minVideoDurationCfg);
            var maxVideoDurationSec = int.Parse(maxVideoDurationCfg);

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
