using Codecaine.Common.Storage.Factory;
using Codecaine.Common.Storage.Providers.AmazonS3.Wrapper;
using Codecaine.Common.Storage.Providers.AzureBlob;
using Codecaine.Common.Storage.Providers.Minio;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Tests.IntegrationTests.Storage
{
    [TestFixture]
    internal class StorageProviderFactoryTests
    {
        /// <summary>
        /// Test for UploadFileAsync method in AzureBlobProvider
        /// Install Docker Azurite Storage Emulator
        ///     - docker pull mcr.microsoft.com/azure-storage/azurite
        ///     - docker run -p 10000:10000 -p 10001:10001 -p 10002:10002  mcr.microsoft.com/azure-storage/azurite
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UploadFileAsync_IsSuccess_WhenCalledAzureBlobStorageProvider()
        {
            // Arrange
            string blobName = "testfileAzure.txt";
            string fileContent = "Hello, Azure Blob!";

            using var loggerFactory = LoggerFactory.Create(builder => { });
            var byteArray = Encoding.UTF8.GetBytes(fileContent);
            using MemoryStream stream = new MemoryStream(byteArray);

            Dictionary<string, string> metadata = new Dictionary<string, string>
            {
                { "author", "test-user" },
                { "description", "integration test file" }
            };

            var factory = new StorageProviderFactory(loggerFactory, new AmazonS3UtilityWrapper());
            var azureBlob = new AzureBlobProvider
            (
                "clean",
                "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;AccountName=devstoreaccount1;BlobEndpoint=http://127.0.0.1:11000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:11001/devstoreaccount1;TableEndpoint=http://127.0.0.1:11002/devstoreaccount1;",
                "devstoreaccount1",
                "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="
            );

            // Act
            using var azureProvider = factory.CreateProvider(azureBlob);
            string? result = await azureProvider.Upload(blobName, stream, metadata);
            bool exists = await azureProvider.IsExist(blobName);
            bool isHealthy = await azureProvider.HealthCheckAsync();
            var url = azureProvider.GenerateSecureUrlAsync(blobName, 1);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(exists, Is.True);
                Assert.That(isHealthy, Is.True);
                Assert.That(url, Does.Contain("?"));
            });

            // Cleanup
            var isDeleted = await azureProvider.Delete(blobName);
            Assert.That(isDeleted, Is.True);
        }

        [Test]
        public async Task UploadFileAsync_IsSuccess_WhenCalledMinioProvider()
        {
            // Arrange
            string blobName = "testFileMinio.txt";
            string fileContent = "Hello, Minio!";

            using var loggerFactory = LoggerFactory.Create(builder => { });
            var byteArray = Encoding.UTF8.GetBytes(fileContent);
            using MemoryStream stream = new MemoryStream(byteArray);

            Dictionary<string, string> metadata = new Dictionary<string, string>
            {
                { "author", "test-user" },
                { "description", "integration test file" }
            };

            var factory = new StorageProviderFactory(loggerFactory, new AmazonS3UtilityWrapper());
            var azureBlob = new MinioProvider
            (

                 "ipe-logger-configs-dev",
                 "YjHh44F6ioMGKIXAT8W5", // Replace with your Minio Access Key
                 "l6xBGjEdwQ4tJNNyOtsd5Zw2nKmQRIpmz5rp1Pis", // Replace with your Minio Secret Key
                 "east-asia",
                 "http://localhost:19000"
            );

            // Act
            using var minioProvider = factory.CreateProvider(azureBlob);
            string? result = await minioProvider.Upload(blobName, stream, metadata);
            bool exists = await minioProvider.IsExist(blobName);
            bool isHealthy = await minioProvider.HealthCheckAsync();
            var url = minioProvider.GenerateSecureUrlAsync(blobName, 1);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(exists, Is.True);
                Assert.That(isHealthy, Is.True);
                Assert.That(url, Does.Contain("?"));
            });

            // Cleanup
             var isDeleted = await minioProvider.Delete(blobName);
            Assert.That(isDeleted, Is.True);
        }
    }
}
