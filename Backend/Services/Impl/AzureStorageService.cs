using System.Drawing;
using System.Drawing.Imaging;

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Backend.Services;

public class AzureStorageService : IStorageService {
    BlobServiceClient _blobServiceClient;

    const string _picturesBlobContainerName = "platoroutephotos";

    public AzureStorageService(string connectionString) {
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadPictureBase64(string pictureBase64) {
        string name = Guid.NewGuid() + ".jpg";

        BlobContainerClient? container =
            _blobServiceClient.GetBlobContainerClient(_picturesBlobContainerName);
        await container.CreateIfNotExistsAsync();

        byte[] imageBytes = Convert.FromBase64String(pictureBase64);
        
        await container
            .UploadBlobAsync(name, new BinaryData(imageBytes));

        return $"{container.Uri}/{name}";
    }
}