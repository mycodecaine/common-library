namespace Codecaine.Common.Storage
{
    public interface IStorageProvider : IDisposable
    {

        /// <summary>
        /// Uploads a file to the specified bucket and returns the MD5 hash of the uploaded file.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="blobName">The name of the blob.</param>
        /// <param name="fileStream">The file stream to upload.</param>
        /// <param name="fileUploadMetadata">Metadata for the file upload.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the MD5 hash of the uploaded file or null if the upload failed.</returns>
        Task<string?> Upload(string blobName, Stream fileStream, Dictionary<string, string>? fileUploadMetadata);

        /// <summary>
        /// Downloads a file from the specified bucket.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="blobName">The name of the blob.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the file stream.</returns>
        Task<Stream> Download(string blobName);

        /// <summary>
        /// Downloads a file from the specified bucket.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="blobName">The name of the blob.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the file stream.</returns>
        Task<(Stream, long)> DownloadWithLength(string blobName);

        /// <summary>
        /// Deletes a file from the specified bucket.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="blobName">The name of the blob.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.</returns>
        Task<bool> Delete(string blobName);

        /// <summary>
        /// Performs a health check on the cloud storage service.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating the health status.</returns>
        Task<bool> HealthCheckAsync();

        /// <summary>
        /// Checks if a file exists in the specified bucket.
        /// </summary>
        /// <param name="bucketName">The name of the bucket.</param>
        /// <param name="blobName">The name of the blob.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the file exists.</returns>
        Task<bool> IsExist(string blobName);

        /// <summary>
        /// Generates a secure URL for accessing a file.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="expiryInMinutes">The expiry time in minutes.</param>
        /// <returns>A secure URL for accessing the file.</returns>
        string GenerateSecureUrlAsync(string fileName, int expiryInMinutes);
    }
}
