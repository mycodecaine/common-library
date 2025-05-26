using Amazon.S3;
using Codecaine.Common.Storage.Providers.AmazonS3;
using Codecaine.Common.Storage.Providers.AmazonS3.Wrapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Storage.Providers.DigitalOceanSpace
{
    public class DigitalOceanSpaceStorageProvider : AmazonS3StorageProvider
    {
        public DigitalOceanSpaceStorageProvider(DigitalOceanSpaceProvider provider, ILogger<DigitalOceanSpaceStorageProvider> logger, IAmazonS3UtilityWrapper amazonS3UtilityWrapper, IAmazonS3 s3Client) : base(provider, logger, amazonS3UtilityWrapper, s3Client)
        {
        }
    }
}
