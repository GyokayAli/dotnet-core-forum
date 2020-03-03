using Forum.Data;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Forum.Service
{
    public class UploadService : IUpload
    {
        #region "Public Methods"

        /// <summary>
        /// Gets the blob container.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="containerName">The Azure container name.</param>
        /// <returns></returns>
        public CloudBlobContainer GetBlobContainer(string connectionString, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            return blobClient.GetContainerReference(containerName);
        }
        #endregion
    }
}
