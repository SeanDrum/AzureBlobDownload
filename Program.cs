using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobDownload
{
    class AzureBlobDownload
    {
        static async Task Main()
        {
            string uriString = ConfigurationManager.AppSettings.Get("SasUri");
            string localDownloadPath = ConfigurationManager.AppSettings.Get("LocalDownloadPath");

            Uri sasUri = new Uri(uriString);

            BlobContainerClient blobContainerClient = new BlobContainerClient(sasUri);

            await foreach (BlobItem blob in blobContainerClient.GetBlobsAsync())
            {
                string fileName = blob.Name;
                string localFilePath = Path.Combine(localDownloadPath, fileName);

                using (var file = File.Open(localFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    var blobClient = blobContainerClient.GetBlobClient(blob.Name);
                    await blobClient.DownloadToAsync(file);

                }
            }
        }
    }
}
