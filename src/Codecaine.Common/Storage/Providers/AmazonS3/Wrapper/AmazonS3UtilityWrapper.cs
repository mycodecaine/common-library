using Amazon.S3;
using Amazon.S3.Util;

namespace Codecaine.Common.Storage.Providers.AmazonS3.Wrapper
{
    public class AmazonS3UtilityWrapper : IAmazonS3UtilityWrapper
    {
        public async Task<bool> DoesS3BucketExistAsync(IAmazonS3 s3Client, string bucketName)
        {
            return await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
        }
    }
}
