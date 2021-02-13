using AdOut.Planning.Model.Interfaces.Services;
using FFMpegCore;
using FFMpegCore.FFMPEG;
using System;
using System.Drawing.Imaging;
using System.IO;
using static AdOut.Planning.Model.Constants;

namespace AdOut.Planning.Core.Services.Content
{
    public class VideoService : IContentService
    {
        public Stream GetThumbnail(Stream content, int width, int height)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentException("Width and height can't be zero and less than zero.");
            }

            //todo: move the RootDirectory path to the appsettings
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
                new System.Drawing.Size(width, height),
                TimeSpan.FromSeconds(DefaultValues.DefaultSecForVideoThumbnail
            ));

            var thumbnailStream = new MemoryStream();
            thumbnail.Save(thumbnailStream, ImageFormat.Png);
            thumbnailStream.Position = 0;

            return thumbnailStream;
        }
    }
}
