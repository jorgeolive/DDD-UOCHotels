using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using UOCHotels.RoomServiceManagement.Application.Services;
using UOCHotels.RoomServiceManagement.AzureStorage.Configuration;

namespace UOCHotels.RoomServiceManagement.AzureStorage
{
    public class AzureStorageService : IFileStorageService
    {
        private readonly AzureStorageConfiguration _config;
        public AzureStorageService(AzureStorageConfiguration config)
        {
            _config = config;
        }

        public async Task<Uri> SaveFile(string fileName, Stream file)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(_config.ConnectionString);
            CloudBlobClient serviceClient = account.CreateCloudBlobClient();

            var container = serviceClient.GetContainerReference("roomservicemanagement");
            container.CreateIfNotExistsAsync().Wait();

            CloudBlockBlob blob = container.GetBlockBlobReference($"roomincidentresources/{fileName}");
            await blob.UploadFromStreamAsync(file);

            return blob.Uri;
        }
    }
}