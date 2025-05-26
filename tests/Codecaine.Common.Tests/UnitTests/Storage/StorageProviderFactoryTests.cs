using Codecaine.Common.Storage.Exceptions;
using Codecaine.Common.Storage.Factory;
using Codecaine.Common.Storage.Providers.Abstractions;
using Codecaine.Common.Storage.Providers.AmazonS3.Wrapper;
using Codecaine.Common.Storage.Providers.AmazonS3;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Codecaine.Common.Storage.Providers.AzureBlob;

namespace Codecaine.Common.Tests.UnitTests.Storage
{
    public class StorageProviderFactoryTests
    {
        private Mock<ILoggerFactory> _loggerFactoryMock;      
        private Mock<IAmazonS3UtilityWrapper> _s3WrapperMock;
        private StorageProviderFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _loggerFactoryMock = new Mock<ILoggerFactory>();            

            _s3WrapperMock = new Mock<IAmazonS3UtilityWrapper>();

            _factory = new StorageProviderFactory(_loggerFactoryMock.Object, _s3WrapperMock.Object);
        }

        [Test]
        public void CreateProvider_WithNullProvider_ThrowsStorageException()
        {
            Assert.Throws<StorageException>(() => _factory.CreateProvider<AmazonS3Provider>(null));
        }

        [Test]
        public void CreateProvider_ReturnsAmazonS3StorageProvider_WhenProviderIsAmazonS3()
        {
            var provider = new AmazonS3Provider("test", "test", "test", "test", "https://testtest.digitaloceanspaces.com");            

            var result = _factory.CreateProvider(provider);

            Assert.That(result, Is.InstanceOf<AmazonS3StorageProvider>());
        }

        [Test]
        public void CreateProvider_ReturnsAzureBlobStorageProvider_WhenProviderIsAzureBlob()
        {
            var provider = new AzureBlobProvider("test", "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;AccountName=devstoreaccou", "test","test");          

            var result = _factory.CreateProvider(provider);

            Assert.That(result, Is.InstanceOf<AzureBlobStorageProvider>());
        }



        [Test]
        public void CreateProvider_WithUnknownProviderType_ThrowsStorageException()
        {
            var unknownProvider = new UnknownProvider();

            var ex = Assert.Throws<StorageException>(() => _factory.CreateProvider(unknownProvider));
            Assert.That(ex.Message, Does.Contain("No factory registered"));
        }

        public class UnknownProvider : Provider { }
    }
}
