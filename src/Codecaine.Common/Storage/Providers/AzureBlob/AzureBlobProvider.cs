using Codecaine.Common.Primitives.Ensure;
using Codecaine.Common.Storage.Providers.Abstractions;

namespace Codecaine.Common.Storage.Providers.AzureBlob
{
    public class AzureBlobProvider : Provider
    {
        public string ConnectionString { get; }
        public string AccountName { get; }
        public string AccountKey { get; }

        /// <summary>
        /// Azure Blob Storage Provider
        /// </summary>
        /// <param name="storageLocation"></param>
        /// <param name="connectionString"></param>
        /// <param name="accountName"></param>
        /// <param name="accountKey"></param>
        public AzureBlobProvider(string storageLocation, string connectionString, string accountName, string accountKey)
        {
            Ensure.NotEmpty(connectionString, "Connection String Is Required", nameof(ConnectionString));
            Ensure.NotEmpty(accountName, "Account Name Is Required", nameof(AccountName));
            Ensure.NotEmpty(accountKey, "Account Key Is Required", nameof(AccountKey));
            Ensure.NotEmpty(storageLocation, "Storage Location Is Required", nameof(StorageLocation));
            StorageLocation = storageLocation;
            ConnectionString = connectionString;
            AccountName = accountName;
            AccountKey = accountKey;

        }

    }
}
