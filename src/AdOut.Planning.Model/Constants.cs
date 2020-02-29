using System.Collections.Generic;

namespace AdOut.Planning.Model
{
    public static class Constants
    {
        public static class ValidationMessages
        {
            public const string NotCorrectFormat = "Format is not correct";
            public const string NotCorrectSize = "Size is not correct";
            public const string NotCorrectDimension = "Dimension is not correct";
            public const string NotCorrectDuration = "Duration is not correct";
        }

        public static class ContentExtensions
        {
            public const string JPEG = ".jpeg";
            public const string JPG = ".jpg";
            public const string PNG = ".png";
            public const string AVI = ".avi";
            public const string MP4 = ".mp4";
        }

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
            public const int Kb = 1024;
            public const int Mb = Kb * 1024;
            public const int Gb = Mb * 1024;
        }

        public static class ConfigurationsTypes
        {
            public const string ImageDimension = "MinImageDimension";
            public const string ImageSize = "MaxImageSizeKb";
        }
    }
}
