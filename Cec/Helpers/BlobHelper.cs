using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace Cec.Helpers
{
    public class BlobHelper
    {
        public async Task<string> UploadAsync(HttpPostedFile image, string containerName)
        {
            try
            {
                string path = image.FileName;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(path);
                CloudBlobContainer container = await GetBlobContainerAsync(containerName);
                CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
                blob.Properties.ContentType = image.ContentType;
                await blob.UploadFromStreamAsync(image.InputStream);
                return blob.Uri.ToString();
            }
            catch (StorageException)
            {
                //If you are running with the default configuration please make sure you have started the storage emulator. Press the Windows key and type Azure Storage to select and run it from the list of applications
                return "ERROR";
            }
        }

        public async void DeleteImage(string uri, string containerName)
        {
            try
            {
                CloudBlobContainer container = await GetBlobContainerAsync(containerName);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(uri);
                await blockBlob.DeleteIfExistsAsync();
            }
            catch (StorageException)
            {
                throw;
            }
        }

        private string GetConnection()
        {
            return ConfigurationManager.ConnectionStrings["PrimaryBlob"].ConnectionString;
        }

        private static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                //Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid
                throw;
            }
            catch (ArgumentException)
            {
                //Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid
                throw;
            }

            return storageAccount;
        }

        public async Task<string> GetPossibleUriAsync(HttpPostedFile image, string containerName)
        {
            string fileName = Path.GetFileName(image.FileName);
            CloudBlobContainer container = await GetBlobContainerAsync(containerName);
            string uri = container.Uri + "/" + fileName;
            return uri;
        }

        private async Task<CloudBlobContainer> GetBlobContainerAsync(string containerName)
        {
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(GetConnection());
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            return container;
        }

    }
}