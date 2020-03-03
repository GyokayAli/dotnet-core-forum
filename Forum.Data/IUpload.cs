using Microsoft.WindowsAzure.Storage.Blob;

namespace Forum.Data
{
    public interface IUpload
    {
        CloudBlobContainer GetBlobContainer(string connectionString, string containerName);
    }
}
