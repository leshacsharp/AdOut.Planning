using AdOut.Planning.Model.Interfaces.Helpers;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AdOut.Planning.Core.ContentHelpers
{
    public class ImageHelper : IContentHelper
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

            var image = Image.FromStream(content);
            var thumb = image.GetThumbnailImage(width, height, null, IntPtr.Zero);

            var thumbStream = new MemoryStream();
            thumb.Save(thumbStream, ImageFormat.Jpeg);
            thumbStream.Position = 0;

            return thumbStream;
        }
    }
}
