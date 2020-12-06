using AdOut.Planning.Model.Enum;
using System;

namespace AdOut.Planning.Core.Helpers
{
    public static class PathHelper
    {
        public static string GeneratePath(string extension, DirectoryPath directoryPath)
        {
            var directoryName = directoryPath switch
            {
                DirectoryPath.None => string.Empty,
                DirectoryPath.Year => DateTime.UtcNow.Year.ToString(),
                DirectoryPath.YearMonth => $"{DateTime.UtcNow.Year}/{DateTime.UtcNow.Month}",
                _ => throw new ArgumentException("invalid enum value", nameof(directoryPath))
            };

            var fileName = Guid.NewGuid();
            var validExtension = extension.StartsWith('.') ? extension : $".{extension}";
            return $"{directoryName}/{fileName}{validExtension}";
        }
    }
}
