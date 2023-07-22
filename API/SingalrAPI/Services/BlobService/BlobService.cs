using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using SingalrAPI.Services.UserService;
using System;
using System.IO;
using System.Threading.Tasks;
using static Azure.Storage.Blobs.BlobClientOptions;

namespace SingalrAPI.Services.BlobService
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobService(IConfiguration configuration)
        {

            var storageAccountName = configuration.GetSection("AzureBlobStorageAccount").Value;

            var AzureBlobStorageSAS = configuration.GetSection("AzureBlobStorageSAS").Value;

            AzureSasCredential sasCredential = new AzureSasCredential(AzureBlobStorageSAS);

            _blobServiceClient = new BlobServiceClient(new Uri("https://"+ storageAccountName + ".blob.core.windows.net"), sasCredential);

            _containerName = "1wxsafiles";

        }
        public async Task UploadBlobAsync(string blobName, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content, true);
        }
        public async Task<List<string>> GetAllBlobs()
        {
            //this is not the best way to do this
            List<string> files = new List<string>();
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                string userId = blobItem.Name.Split('_')[0];
                string userName = AppUser.appUsers.Where(u => u.Id == userId).FirstOrDefault()?.UserName;

                if (!string.IsNullOrEmpty(userName))
                {

                    files.Add(blobItem.Name.Replace(userId, userName));
                }
                else
                {
                    files.Add(blobItem.Name);

                }


            }
            return files;
        }
        public async Task<Stream> DownloadBlobAsync(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
