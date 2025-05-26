using Codecaine.Common.Storage.Providers.AmazonS3;

namespace Codecaine.Common.Storage.Providers.DigitalOceanSpace
{
    public class DigitalOceanSpaceProvider : AmazonS3Provider
    {
        /// <summary>
        /// Digital Ocean Space Storage Provider
        /// </summary>
        /// <param name="storageLocation"></param>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="region"></param>
        /// <param name="endpoint"></param>
        public DigitalOceanSpaceProvider(string storageLocation, string accessKey, string secretKey, string region, string endpoint) : base(storageLocation, accessKey, secretKey, region, endpoint)
        {
        }
    }
}
