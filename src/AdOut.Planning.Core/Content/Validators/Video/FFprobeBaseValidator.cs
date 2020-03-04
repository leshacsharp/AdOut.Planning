using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Repositories;
using Alturos.VideoInfo;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Content.Validators.Video
{
    public abstract class FFprobeBaseValidator : VideoTemplateValidator
    {
        private readonly IConfigurationRepository _configurationRepository;

        public FFprobeBaseValidator(IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
        }

        protected override async Task<bool> IsCorrectDimensionAsync(Stream content)
        {
            var minVideoDimensionConfig = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MinVideoDimension);
            var dimensionParts = minVideoDimensionConfig.Split('x', StringSplitOptions.RemoveEmptyEntries);

            if (dimensionParts.Length != 2)
                throw new ConfigurationException("Invalid video dimesion config");

            var minVideoWidth = int.Parse(dimensionParts[0]);
            var minVideoHeight = int.Parse(dimensionParts[1]);

            var videoInfo = await GetVideoInfoAsync(content);  
            return videoInfo.Width >= minVideoWidth && videoInfo.Height >= minVideoHeight;
        }

        protected override async Task<bool> IsCorrectSizeAsync(Stream content)
        {
            var maxVideoSizeConfig = await _configurationRepository.GetByTypeAsync(ConfigurationsTypes.MaxVideoSize);
            var maxVideoSizeMb = int.Parse(maxVideoSizeConfig);

            var imageSizeMb = content.Length / ContentSizes.Mb;
            return imageSizeMb <= maxVideoSizeMb;
        }

        protected override async Task<bool> IsCorrectDurationAsync(Stream content)
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
            var analyzerResult = videoAnalyzer.GetVideoInfo(videoBuffer);

            var videoStream = analyzerResult.VideoInfo.Streams.Single(s => s.CodecType == CodecTypes.Video);
            return videoStream;
        }
    }
}
