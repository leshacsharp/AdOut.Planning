using AdOut.Planning.Model.Interfaces.Services;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AdOut.Planning.Core.Services.Content
{
    public class AWSS3Storage : IContentStorage
    {
        private readonly IAmazonS3 _awsClient;
        private readonly string _bucketName;

        public AWSS3Storage(IAmazonS3 awsClient, string bucketName)
        {
            _awsClient = awsClient;
            _bucketName = bucketName;
        }

        public Task CreateAsync(Stream content, string filePath)
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
                BucketName = _bucketName,
                Key = filePath,
                InputStream = content
            };

            return _awsClient.PutObjectAsync(putRequest);
        }

        public Task DeleteAsync(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(filePath);
            }

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = filePath,
            };

            return _awsClient.DeleteObjectAsync(deleteRequest);
        }

        public async Task<Stream> GetAsync(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(filePath);
            }

            var getRequest = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = filePath,
            };

            var response =  await _awsClient.GetObjectAsync(getRequest);
            return response.ResponseStream;
        }
    }
}
