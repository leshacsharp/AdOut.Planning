using AdOut.Planning.Common.Helpers;
using AdOut.Planning.Model.Interfaces.Content;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Content.Storages
{
    public class LocalStorage : IContentStorage
    {
        private readonly IDirectoryDistributor _directorySeparator;
        public LocalStorage(IDirectoryDistributor directorySeparator)
        {
            _directorySeparator = directorySeparator;
        }

        public string GenerateFilePath(string fileExtension)
        {
            if (fileExtension == null)
            {
                throw new ArgumentNullException(nameof(fileExtension));
            }

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var subDirectory = _directorySeparator.GetDirectory();
            var fileName = FileHelper.GetRandomFileName();
            var fullPath = $"{baseDirectory}/{subDirectory}/{fileName}{fileExtension}";

            return fullPath;
        }

        public Task CreateObjectAsync(Stream content, string filePath)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            using (var fileStream = File.Create(filePath))
            {
                return content.CopyToAsync(fileStream);
            }
        }

        public Task DeleteObjectAsync(string filePath)
        {
            ValidateFilePath(filePath);
            File.Delete(filePath);

            return Task.CompletedTask;
        }

        public Task<Stream> GetObjectAsync(string filePath)
        {
            ValidateFilePath(filePath);
            var stream = (Stream)File.OpenRead(filePath);

            return Task.FromResult(stream);
        }

        private void ValidateFilePath(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File with path={filePath} was not found", filePath);
            }
        } 
    }
}
