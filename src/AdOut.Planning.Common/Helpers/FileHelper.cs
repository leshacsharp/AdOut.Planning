using System;

namespace AdOut.Planning.Common.Helpers
{
    public static class FileHelper
    {
        public static string GetRandomFileName()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
