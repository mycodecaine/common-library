using Amazon.S3;
using Azure.Storage.Blobs;
using Codecaine.Common.Storage.Exceptions;
using Codecaine.Common.Storage.Providers.Abstractions;
using Codecaine.Common.Storage.Providers.AmazonS3;
using Codecaine.Common.Storage.Providers.AmazonS3.Wrapper;
using Codecaine.Common.Storage.Providers.AzureBlob;
using Codecaine.Common.Storage.Providers.DigitalOceanSpace;
using Codecaine.Common.Storage.Providers.Minio;
using Microsoft.Extensions.Logging;

namespace Codecaine.Common.Storage.Factory
{
    public class StorageProviderFactory : IStorageProviderFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IAmazonS3UtilityWrapper _s3UtilityWrapper;
        private readonly Dictionary<Type, Func<Provider, ILoggerFactory, IStorageProvider>> _providerFactories;

        public StorageProviderFactory(ILoggerFactory loggerFactory, IAmazonS3UtilityWrapper s3UtilityWrapper)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _s3UtilityWrapper = s3UtilityWrapper ?? throw new ArgumentNullException(nameof(s3UtilityWrapper));
            _providerFactories = new Dictionary<Type, Func<Provider, ILoggerFactory, IStorageProvider>>
            {
            
                // Azure Blob Storage Provider
                { typeof(AzureBlobProvider), (provider, loggerFactory) => new AzureBlobStorageProvider((AzureBlobProvider)provider,
                        loggerFactory.CreateLogger<AzureBlobStorageProvider>(), GetClient((AzureBlobProvider)provider)) },

                // AWS S3 Storage Provider
                { typeof(AmazonS3Provider), (provider, loggerFactory) => new AmazonS3StorageProvider((AmazonS3Provider)provider,
                        loggerFactory.CreateLogger<AmazonS3StorageProvider>(), _s3UtilityWrapper, GetClient((AmazonS3Provider)provider)) },

                // Minio Storage Provider
                { typeof(MinioProvider), (provider, loggerFactory) => new MinioStorageProvider((MinioProvider)provider,
                        loggerFactory.CreateLogger<MinioStorageProvider>(), _s3UtilityWrapper, GetClient((MinioProvider)provider)) },

                // DigitalOcean Space Storage Provider
                { typeof(DigitalOceanSpaceProvider), (provider, loggerFactory) => new DigitalOceanSpaceStorageProvider((DigitalOceanSpaceProvider)provider,
                        loggerFactory.CreateLogger<DigitalOceanSpaceStorageProvider>(),_s3UtilityWrapper, GetClient((DigitalOceanSpaceProvider)provider)) },

            };
            _s3UtilityWrapper = s3UtilityWrapper;
        }
        public IStorageProvider CreateProvider<T>(T provider) where T : Provider
        {
            if (provider == null) throw new StorageException(nameof(provider));

            if (_providerFactories.TryGetValue(typeof(T), out var factory))
            {
                return factory(provider, _loggerFactory);
            }

            throw new StorageException($"No factory registered for provider type {typeof(T).Name}");
        }

        /// <summary>
        /// Get Amazon S3 Client
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        private static IAmazonS3 GetClient(AmazonS3Provider provider)
        {
            var config = new AmazonS3Config
            {
                ServiceURL = provider.Endpoint,
                ForcePathStyle = provider.ForcePathStyle
            };
            return new AmazonS3Client(provider.AccessKey, provider.SecretKey, config);
        }

        /// <summary>
        /// Get Azure Blob Client
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        private static BlobServiceClient GetClient(AzureBlobProvider provider)
        {
            return new BlobServiceClient(provider.ConnectionString);
        }


    }
}
