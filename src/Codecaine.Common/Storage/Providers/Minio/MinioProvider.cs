using Codecaine.Common.Storage.Providers.AmazonS3;

namespace Codecaine.Common.Storage.Providers.Minio
{
    public class MinioProvider : AmazonS3Provider
    {
        /// <summary>
        /// Minio Storage Provider
        /// </summary>
        /// <param name="storageLocation"></param>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="region"></param>
        /// <param name="endpoint"></param>
        public MinioProvider(string storageLocation, string accessKey, string secretKey, string region, string endpoint) : base(storageLocation, accessKey, secretKey, region, endpoint)
        {
        }
        public override bool ForcePathStyle { get; } = true;
    }
}
