using Amazon.S3;

namespace Codecaine.Common.Storage.Providers.AmazonS3.Wrapper
{
    public interface IAmazonS3UtilityWrapper
    {
        Task<bool> DoesS3BucketExistAsync(IAmazonS3 s3Client, string bucketName);
    }
}
