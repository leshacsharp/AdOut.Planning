using AdOut.Planning.Common.Helpers;
using AdOut.Planning.Core.Settings;
using AdOut.Planning.Model.Interfaces.Content;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Content.Storages
{
    public class AWSS3Storage : IContentStorage
    {
        private readonly IDirectoryDistributor _directorySeparator;
        private readonly IAmazonS3 _awsClient;
        private readonly AWSS3Config _awsConfig;

        public AWSS3Storage(IDirectoryDistributor directorySeparator, IOptions<AWSS3Config> awsConfig)
        {
            _directorySeparator = directorySeparator;
            _awsConfig = awsConfig.Value;

            var awsCredentials = new BasicAWSCredentials(_awsConfig.AccessKey, _awsConfig.SecretKey);
            var regionEndpoint = RegionEndpoint.GetBySystemName(_awsConfig.RegionEndpointName);

            _awsClient = new AmazonS3Client(awsCredentials, regionEndpoint);
        }

        public string GenerateFilePath(string fileExtension)
        {
            if (fileExtension == null)
            {
                throw new ArgumentNullException(fileExtension);
            }

            var directory = _directorySeparator.GetDirectory();
            var fileName = FileHelper.GetRandomFileName();
            var fullPath = $"{directory}/{fileName}{fileExtension}";

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
                throw new ArgumentNullException(filePath);
            }

            var putRequest = new PutObjectRequest
            {
                BucketName = _awsConfig.BucketName,
                Key = filePath,
                InputStream = content
            };

            //todo: add logging
            return _awsClient.PutObjectAsync(putRequest);
        }

        public Task DeleteObjectAsync(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(filePath);
            }

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _awsConfig.BucketName,
                Key = filePath,
            };

            //todo: add logging
            return _awsClient.DeleteObjectAsync(deleteRequest);
        }

        public async Task<Stream> GetObjectAsync(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(filePath);
            }

            var getRequest = new GetObjectRequest
            {
                BucketName = _awsConfig.BucketName,
                Key = filePath,
            };

            //todo: add logging
            var response =  await _awsClient.GetObjectAsync(getRequest);
            return response.ResponseStream;
        }
    }
}
