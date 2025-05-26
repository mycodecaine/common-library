using Codecaine.Common.Primitives.Ensure;
using Codecaine.Common.Storage.Providers.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Storage.Providers.AmazonS3
{
    public class AmazonS3Provider : Provider
    {
        public string AccessKey { get; }
        public string SecretKey { get; }
        public string Region { get; }
        public string Endpoint { get; }
        public virtual bool ForcePathStyle { get; } = false;

        /// <summary>
        ///  Aws S3 Storage Provider
        /// </summary>
        /// <param name="storageLocation"></param>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="region"></param>
        /// <param name="endpoint"></param>
        public AmazonS3Provider(string storageLocation, string accessKey, string secretKey, string region, string endpoint)
        {

            Ensure.NotEmpty(accessKey, "Access Key Is Required", nameof(AccessKey));
            Ensure.NotEmpty(accessKey, "Access Key Is Required", nameof(AccessKey));
            Ensure.NotEmpty(secretKey, "Secret Key Is Required", nameof(SecretKey));
            Ensure.NotEmpty(region, "Region Is Required", nameof(Region));
            Ensure.NotEmpty(endpoint, "Endpoint Is Required", nameof(Endpoint));

            StorageLocation = storageLocation;
            AccessKey = accessKey;
            SecretKey = secretKey;
            Region = region;
            Endpoint = endpoint;

        }
    }
}
