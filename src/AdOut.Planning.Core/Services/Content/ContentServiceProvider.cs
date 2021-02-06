using AdOut.Planning.Model;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Services;
using System;

namespace AdOut.Planning.Core.Services.Content
{
    public class ContentServiceProvider : IContentServiceProvider
    { 
        public IContentService CreateContentService(string contentExtension)
        {
            if (contentExtension == null)
            {
                throw new ArgumentNullException(nameof(contentExtension));
            }

            if (!Constants.ContentTypes.TryGetValue(contentExtension, out ContentType contentType))
            {
                throw new NotSupportedException($"Extension={contentExtension} is not supported");
            }

            return contentType switch
            {
                ContentType.Image => new ImageService(),
                ContentType.Video => new VideoService(),
                _ =>  throw new ArgumentException("Invalid enum value")
            };
        }
    }
}
