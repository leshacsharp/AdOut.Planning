﻿using AdOut.Planning.Model.Classes;
using AdOut.Planning.Model.Interfaces.Repositories;
using AdOut.Planning.Model.Interfaces.Services;
using Alturos.VideoInfo;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Validators.Base
{
    public abstract class VideoBaseValidator : IContentValidator
    {
        private readonly IConfigurationRepository _configurationRepository;
        public VideoBaseValidator(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        public async Task<ValidationResult<string>> ValidateAsync(Stream content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var isCorrectFormat = await IsCorrectFormatAsync(content);
            if (!isCorrectFormat)
            {
                //throwing an exception because the validators can't validate a content with an invalid format
                throw new ArgumentException(ValidationMessages.Content.InvalidFormat, nameof(content));
            }

            var validationResult = new ValidationResult<string>();
            var isCorrectDimension = await IsCorrectDimensionAsync(content);
            var isCorrectSize = await IsCorrectSizeAsync(content);
            var isCorrectDuration = await IsCorrectDurationAsync(content);

            if (!isCorrectDimension)
            {
                validationResult.Errors.Add(ValidationMessages.Content.NotCorrectDimension);
            }
            if (!isCorrectSize)
            {
                validationResult.Errors.Add(ValidationMessages.Content.NotCorrectSize);
            }
            if (!isCorrectDuration)
            {
                validationResult.Errors.Add(ValidationMessages.Content.NotCorrectDuration);
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