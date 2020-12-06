using AdOut.Planning.Model;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Content;
using System;

namespace AdOut.Planning.Core.Content.Helpers
{
    public class ContentHelperProvider : IContentHelperProvider
    { 
        public IContentHelper CreateContentHelper(string contentExtension)
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
                ContentType.Image => new ImageHelper(),
                ContentType.Video => new VideoHelper(),
                _ =>  throw new ArgumentException("Invalid enum value")
            };
        }
    }
}
