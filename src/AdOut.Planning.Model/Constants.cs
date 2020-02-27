namespace AdOut.Planning.Model
{
    public static class Constants
    {
        public static class ValidationMessages
        {
            public const string NotCorrectFormat = "Format is not correct";
            public const string NotCorrectSize = "Size is not correct";
            public const string NotCorrectDuration = "Duration is not correct";
        }

        public static class ContentExtensions
        {
            public const string JPEG = ".jpeg";
            public const string JPG = ".jpg";
            public const string PNG = ".png";
        }

        public static class ContentSignature
        {
            public static byte[] JPEG = new byte[] { 0xff, 0xd8, 0xff };
            public static byte[] PNG = new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a };
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
