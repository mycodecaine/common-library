using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Codecaine.Common.Storage.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Storage.Providers.AzureBlob
{
    public class AzureBlobStorageProvider : IStorageProvider
    {

        protected readonly AzureBlobProvider _provider;
        protected BlobServiceClient _blobServiceClient;
        private readonly Lazy<BlobContainerClient> _blobContainerClient;
        protected readonly ILogger<AzureBlobStorageProvider> _logger;
        private bool _disposed = false; // To track whether Dispose has been called

        public AzureBlobStorageProvider(AzureBlobProvider azureBlob, ILogger<AzureBlobStorageProvider> logger, BlobServiceClient blobServiceClient)
        {
            _provider = azureBlob;
            _logger = logger;
            _blobServiceClient = blobServiceClient;
            // Lazy initialize BlobContainerClient
            _blobContainerClient = new Lazy<BlobContainerClient>(() =>
                _blobServiceClient.GetBlobContainerClient(_provider.StorageLocation));

        }

        // Accessor to retrieve the lazily initialized container client
        private BlobContainerClient BlobContainerClient => _blobContainerClient.Value;


        public async Task<bool> Delete(string blobName)
        {
            var blobClient = BlobContainerClient.GetBlobClient(blobName);
            var response = await blobClient.DeleteIfExistsAsync();

            return response.Value;
        }

        public async Task<Stream> Download(string blobName)
        {

            var blobClient = BlobContainerClient.GetBlobClient(blobName);

            if (blobClient == null)
            {
                throw new StorageException($"Blob {blobName} does not exist.");
            }

            var isExist = await blobClient.ExistsAsync();
            if (!isExist)
            {
                throw new StorageException($"Blob {blobName} does not exist.");
            }

            var downloadInfo = await blobClient.DownloadAsync();
            return downloadInfo.Value.Content;
        }

        public string GenerateSecureUrlAsync(string fileName, int expiryInMinutes)
        {

            var expiryTime = DateTime.UtcNow.AddMinutes(expiryInMinutes);
            var permissions = BlobSasPermissions.Read;

            // Create the SAS token
            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _provider.StorageLocation,
                BlobName = fileName,
                Resource = "b", // "c" for container-level, "b" for blob-level SAS
                ExpiresOn = expiryTime,
                // IPRange = new SasIPRange(IPAddress.Parse(GetLocalIPAddress())),
            };
            sasBuilder.SetPermissions(permissions);

            string sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_provider.AccountName, _provider.AccountKey)).ToString();

            return $"{BlobContainerClient.GetBlobClient(fileName).Uri}?{sasToken}";
        }

        public async Task<bool> HealthCheckAsync()
        {
            try
            {
                // Check if connection string is valid
                if (string.IsNullOrWhiteSpace(_provider.ConnectionString))
                {
                    throw new StorageException("Connection string is missing or empty.");
                }

                // Create BlobServiceClient
                var properties = await _blobServiceClient.GetPropertiesAsync();               
                _logger.LogInformation("Storage Account Connected: {DefaultServiceVersion}", properties.Value.DefaultServiceVersion);


                return true;
            }
            catch (Exception ex)
            {
               
                _logger.LogError(ex,"Configuration validation failed: {Message}",ex.Message);
                return false;
            }
        }

        public async Task<bool> IsExist(string blobName)
        {
            var blobClient = BlobContainerClient.GetBlobClient(blobName);
            return await blobClient.ExistsAsync();
        }

        public async Task<string?> Upload(string blobName, Stream fileStream, Dictionary<string, string>? fileUploadMetadata)
        {
            var blobClient = BlobContainerClient.GetBlobClient(blobName);
            var options = new BlobUploadOptions
            {
                Tags = fileUploadMetadata,
            };
           
            var response = await blobClient.UploadAsync(fileStream, options);

            if (response.Value != null)
            {
                return response.Value.ETag.ToString();
            }
            return null;
        }

        // IDisposable implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {               
                _disposed = true;
            }
        }

        public async Task<(Stream, long)> DownloadWithLength(string blobName)
        {
            var blobClient = BlobContainerClient.GetBlobClient(blobName);

            if (blobClient == null)
            {
                throw new StorageException($"Blob {blobName} does not exist.");
            }

            var isExist = await blobClient.ExistsAsync();
            if (!isExist)
            {
                throw new StorageException($"Blob {blobName} does not exist.");
            }

            var downloadInfo = await blobClient.DownloadAsync();
            return (downloadInfo.Value.Content, downloadInfo.Value.Details.ContentLength);
        }

        ~AzureBlobStorageProvider()
        {
            Dispose(false);
        }
    }
}
