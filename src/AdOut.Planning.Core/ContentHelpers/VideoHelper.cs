using AdOut.Planning.Model.Interfaces.Helpers;
using FFMpegCore;
using FFMpegCore.FFMPEG;
using System;
using System.Drawing.Imaging;
using System.IO;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.ContentHelpers
{
    public class VideoHelper : IContentHelper
    {
        public Stream GetThumbnail(Stream content, int width, int height)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            if (width <= 0)
            {
                throw new ArgumentException("Value can't be zero and less zero", nameof(width));
            }
            if (height <= 0)
            {
                throw new ArgumentException("Value can't be zero and less zero", nameof(height));
            }

            var FFMpegOptions = new FFMpegOptions() { RootDirectory = AppDomain.CurrentDomain.BaseDirectory };
            FFMpegOptions.Configure(FFMpegOptions);

            var tempFilePath = Path.GetTempFileName();
            var tempFileInfo = new FileInfo(tempFilePath);

            using (var tempStream = File.OpenWrite(tempFilePath))
            {
                content.CopyTo(tempStream);
            }

            var ffmpeg = new FFMpeg();
            var videoInfo = VideoInfo.FromFileInfo(tempFileInfo);

            var thumbnail = ffmpeg.Snapshot(
                videoInfo,
                tempFileInfo,
                DefaultValues.DefaultThumbnailSize,
                TimeSpan.FromSeconds(DefaultValues.DefaultSecForVideoThumbnail
            ));

            var thumbnailStream = new MemoryStream();
            thumbnail.Save(thumbnailStream, ImageFormat.Png);
            thumbnailStream.Position = 0;

            return thumbnailStream;
        }
    }
}
