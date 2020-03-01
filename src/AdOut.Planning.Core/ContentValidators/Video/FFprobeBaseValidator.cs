using AdOut.Planning.Model.Exceptions;
using AdOut.Planning.Model.Interfaces.Repositories;
using Alturos.VideoInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ContentValidators.Video
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
            var videoDimensionConfig = await _configurationRepository.Read(c => c.Type == ConfigurationsTypes.MinVideoDimension).SingleAsync();
            var dimensionParts = videoDimensionConfig.Value.Split('x', StringSplitOptions.RemoveEmptyEntries);

            if (dimensionParts.Length != 2)
                throw new ConfigurationException("Invalid video dimesion config");

            var minVideoWidth = int.Parse(dimensionParts[0]);
            var minHeightHeight = int.Parse(dimensionParts[1]);

            var videoBuffer = new byte[content.Length];
            await content.ReadAsync(videoBuffer, 0, videoBuffer.Length);

            var videoAnalyzer = new VideoAnalyzer();
            var analyzerResult = videoAnalyzer.GetVideoInfo(videoBuffer);
            var videoInfo = analyzerResult.VideoInfo;
        }

        protected override Task<bool> IsCorrectSizeAsync(Stream content)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> IsCorrectDurationAsync(Stream content)
        {
            throw new NotImplementedException();
        }
    }
}
