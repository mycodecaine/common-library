using Amazon.S3;
using Codecaine.Common.Storage.Providers.AmazonS3;
using Codecaine.Common.Storage.Providers.AmazonS3.Wrapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Storage.Providers.Minio
{
    public class MinioStorageProvider : AmazonS3StorageProvider
    {
        public MinioStorageProvider(MinioProvider provider, ILogger<MinioStorageProvider> logger, IAmazonS3UtilityWrapper amazonS3UtilityWrapper, IAmazonS3 s3Client) : base(provider, logger, amazonS3UtilityWrapper, s3Client)
        {
        }
    }
}
