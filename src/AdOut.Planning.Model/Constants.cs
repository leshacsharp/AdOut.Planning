using System.Collections.Generic;
using AdOut.Planning.Model.Enum;

namespace AdOut.Planning.Model
{
    public static class Constants
    {
        //_T - template
        public static class ValidationMessages
        {
            public static class Schedule
            {
                public const string DayDayOff_T = "Schedule day of week {0} is a day off for AdPoint({1})";
                public const string DateDayOff_T = "Schedule date {0} is a day off for Ad Point({1})";
                public const string TimeNotAllowed_T = "Schedule {0} goes out of AdPoint({1}) schedule bounds {2}";
                public const string TimeIntersected_T = "Schedule intersects an existing schedule at {0} date";
                public const string DateBounds_T = "Schedule date {0} goes out of plan bounds {1}";
                public const string TimeIncreased = "Ads showing time increased compared to previos schedule state, you cannot increase it";
            }

            public static class Content
            {
                public const string InvalidFormat = "Format is invalid";
                public const string NotCorrectSize = "Size is not correct";
                public const string NotCorrectDimension = "Dimension is not correct";
                public const string NotCorrectDuration = "Duration is not correct";
            }
        }

        public static class ContentExtensions
        {
            public const string JPEG = ".jpeg";
            public const string JPG = ".jpg";
            public const string PNG = ".png";
            public const string AVI = ".avi";
            public const string MP4 = ".mp4";
        }

        public static List<string> AllowedExtensions = new List<string>()
        {
            ContentExtensions.JPEG, ContentExtensions.JPG, ContentExtensions.PNG, ContentExtensions.AVI, ContentExtensions.MP4
        };

        public static Dictionary<string, ContentType> ContentTypes = new Dictionary<string, ContentType>()
        {
            { ContentExtensions.JPEG, ContentType.Image },
            { ContentExtensions.JPG, ContentType.Image },
            { ContentExtensions.PNG, ContentType.Image },
            { ContentExtensions.AVI, ContentType.Video },
            { ContentExtensions.MP4, ContentType.Video }
        };

        public static class ContentSignatures
        {
            public static byte[] JPEG_START = new byte[] { 0xff, 0xd8, 0xff };
            public static byte[] JPEG_END = new byte[] { 0xff, 0xd9 };
            public static byte[] PNG = new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a };
            public static byte[] MP4 = new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70 };
            public static List<byte[]> AVI = new List<byte[]>()
            {
                new byte[] { 0x52, 0x49, 0x46, 0x46 },
                new byte[] { 0x41, 0x56, 0x49, 0x20, 0x4C, 0x49, 0x53, 0x54 }
            };
        }

        public static class ContentSizes
        {
            public const int Kb = 1024; //bytes
            public const int Mb = Kb * 1024;
            public const int Gb = Mb * 1024;
        }

        public static class CodecTypes
        {
            public const string Video = "video";
            public const string Audio = "audio";
        }

        public static class ConfigurationsTypes
        {
            public const string MinImageWidth = "MinImageWidth";
            public const string MinImageHeight = "MinImageHeight";
            public const string MinVideoWidth = "MinVideoWidth";
            public const string MinVideoHeight = "MinVideoHeight";
            public const string MaxImageSizeKb = "MaxImageSizeKb";
            public const string MaxVideoSizeKb = "MaxVideoSizeKb";
            public const string MaxVideoDuration = "MaxVideoDurationSec";
            public const string MinVideoDuration = "MinVideoDurationSec";
        }

        public static class DefaultValues
        {
            public const int DefaultThumbnailWidth = 320;
            public const int DefaultThumbnailHeight = 240;
            public const int DefaultSecForVideoThumbnail = 2;
            public const string DefaultThumbnailExtension = ContentExtensions.PNG;
        }

        public static class AuthPolicies
        {
            public const string ResourcePolicy = "ResourcePolicy";
        }
    }
}
