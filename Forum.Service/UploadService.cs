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
        /// <returns></returns>
        public CloudBlobContainer GetBlobContainer(string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            return blobClient.GetContainerReference("profile-images");
        }
        #endregion
    }
}
