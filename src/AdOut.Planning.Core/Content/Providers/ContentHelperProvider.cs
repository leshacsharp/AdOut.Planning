using AdOut.Planning.Core.Content.Helpers;
using AdOut.Planning.Model;
using AdOut.Planning.Model.Enum;
using AdOut.Planning.Model.Interfaces.Content;
using System;

namespace AdOut.Planning.Core.Content.Providers
{
    //todo: create abstract factory with IContentValidatorProvider
    public class ContentHelperProvider : IContentHelperProvider
    { 
        public IContentHelper CreateContentHelper(string contentExtension)
        {
            if (contentExtension == null)
            {
                throw new ArgumentNullException(nameof(contentExtension));
            }

            //todo: refactoring
            if(!Constants.ContentTypes.ContainsKey(contentExtension))
            {
                throw new NotSupportedException($"Extension={contentExtension} is not supported");
            }

            var contentType = Constants.ContentTypes[contentExtension];

            return contentType switch
            {
                ContentType.Image => new ImageHelper(),
                ContentType.Video => new VideoHelper(),
            };
        }
    }
}
