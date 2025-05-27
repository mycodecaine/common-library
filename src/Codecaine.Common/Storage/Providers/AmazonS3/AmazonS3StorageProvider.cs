using Amazon.S3.Model;
using Amazon.S3;
using Codecaine.Common.Storage.Providers.AmazonS3.Wrapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Codecaine.Common.Storage.Exceptions;

namespace Codecaine.Common.Storage.Providers.AmazonS3
{
    public class AmazonS3StorageProvider : IStorageProvider
    {
        protected readonly AmazonS3Provider _provider;
        protected readonly ILogger<AmazonS3StorageProvider> _logger;
        private readonly IAmazonS3 _s3Client;
        private readonly IAmazonS3UtilityWrapper _amazonS3UtilityWrapper;
        private bool _disposed = false; // Track disposal state


        public AmazonS3StorageProvider(AmazonS3Provider provider, ILogger<AmazonS3StorageProvider> logger, IAmazonS3UtilityWrapper amazonS3UtilityWrapper, IAmazonS3 s3Client)
        {
            _provider = provider;
            _logger = logger;
            _s3Client = s3Client;
            _amazonS3UtilityWrapper = amazonS3UtilityWrapper;
        }

        public async Task<bool> Delete(string blobName)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _provider.StorageLocation,
                Key = blobName
            };

            var deleteObjectResponse =  await _s3Client.DeleteObjectAsync(deleteObjectRequest);
            if (deleteObjectResponse.HttpStatusCode != HttpStatusCode.NoContent)
            {
              
                return false;
            }

            return true;
        }

        public async Task<Stream> Download(string blobName)
        {

            var isExist = await IsExist(blobName);
            if (!isExist)
            {
                throw new StorageException($"Blob {blobName} does not exist.");
            }

            var request = new GetObjectRequest
            {
                BucketName = _provider.StorageLocation,
                Key = blobName,
            };

            var getObject = await _s3Client.GetObjectAsync(request);
            return getObject.ResponseStream;
        }

        public string GenerateSecureUrlAsync(string fileName, int expiryInMinutes)
        {
            var dic = new Dictionary<string, object>();
            dic.Add("BucketName", _provider.StorageLocation);

            return _s3Client.GeneratePreSignedURL(_provider.StorageLocation, fileName, DateTime.UtcNow.AddMinutes(expiryInMinutes), dic);
        }

        public async Task<bool> HealthCheckAsync()
        {
            if (string.IsNullOrWhiteSpace(_provider?.StorageLocation))
                return false;

            try
            {
                var response = await _s3Client.ListBucketsAsync();

                bool bucketExists = response.Buckets
                    .Any(bucket => bucket.BucketName.StartsWith(_provider.StorageLocation, StringComparison.OrdinalIgnoreCase));

                return bucketExists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "S3 health check failed.");
                return false;
            }
        }

        public async Task<bool> IsExist(string blobName)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _provider.StorageLocation,
                Prefix = blobName
            };

            var response = await _s3Client.ListObjectsV2Async(request);
            return response.S3Objects.Count > 0;
        }


        public async Task<string?> Upload(string blobName, Stream fileStream, Dictionary<string, string>? fileUploadMetadata)
        {

            // Check if the bucket exists
            var isExist = VerifyBucketExistence();
            if (!isExist.Result)
            {
                _logger.LogError("Bucket {StorageLocation} does not exist.", _provider.StorageLocation);
                throw new StorageException($"Bucket {_provider.StorageLocation} does not exist.");
            }

            var putObjectRequest = new PutObjectRequest
            {
                BucketName = _provider.StorageLocation,
                Key = blobName,
                InputStream = fileStream,
                CannedACL = S3CannedACL.Private
            };

            if (fileUploadMetadata == null)
            {
                return null;
            }

            foreach (var (key, value) in fileUploadMetadata)
            {
                putObjectRequest.Metadata.Add(key, value);
            }

            var putObjectResponse = await _s3Client.PutObjectAsync(putObjectRequest);
            if (putObjectResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                return putObjectResponse.ETag;
            }
            return null;
        }

        /// <summary>
        /// Verifies if the bucket exists, if not creates a new bucket.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> VerifyBucketExistence()
        {
            var isBucketExist = await _amazonS3UtilityWrapper.DoesS3BucketExistAsync(_s3Client, _provider.StorageLocation);

            if (isBucketExist)
            {
                return true;
            }

            if (!isBucketExist)
            {
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = _provider.StorageLocation,
                    UseClientRegion = true
                };
                var response = await _s3Client.PutBucketAsync(putBucketRequest);
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    return true;
                }

            }
            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Prevents the garbage collector from calling the finalizer
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _s3Client?.Dispose();
                }

                _disposed = true;
            }
        }

        public async Task<(Stream, long)> DownloadWithLength(string blobName)
        {
            var isExist = await IsExist(blobName);
            if (!isExist)
            {
                throw new StorageException($"Blob {blobName} does not exist.");
            }

            var request = new GetObjectRequest
            {
                BucketName = _provider.StorageLocation,
                Key = blobName,
            };

            var getObject = await _s3Client.GetObjectAsync(request);
            var responseStream = getObject.ResponseStream;
            return (responseStream, responseStream.Length);
        }

        ~AmazonS3StorageProvider()
        {
            Dispose(false);
        }
    }
}
