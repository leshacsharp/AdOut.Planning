﻿using AdOut.Planning.Model.Interfaces.Content;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AdOut.Planning.Core.Content.Helpers
{
    public class ImageHelper : IContentHelper
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

            var image = Image.FromStream(content);
            var thumbnail = image.GetThumbnailImage(width, height, null, IntPtr.Zero);

            var thumbnailStream = new MemoryStream();
            thumbnail.Save(thumbnailStream, ImageFormat.Png);
            thumbnailStream.Position = 0;

            return thumbnailStream;
        }
    }
}
