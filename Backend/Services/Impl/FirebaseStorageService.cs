using System.Drawing;
using System.Drawing.Imaging;

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Firebase.Storage;

namespace Backend.Services;

public class FirebaseStorageService : IStorageService {

    FirebaseStorage _firebaseStorage;

    const string USER_PHOTOS_CONTAINER = "USER_PHOTOS";

    public FirebaseStorageService(string connectionString, string firebaseToken) {
        FirebaseStorageOptions storageOptions = new FirebaseStorageOptions { AuthTokenAsyncFactory = () => Task.FromResult(firebaseToken) };
        _firebaseStorage = new FirebaseStorage(connectionString, storageOptions);
    }

    public async Task<string> UploadPictureBase64Async(string pictureBase64) {
        string fileName = Guid.NewGuid() + ".jpg";

        byte[] imageBytes = Convert.FromBase64String(pictureBase64);
        Stream imageStream = new MemoryStream(imageBytes);

        string downloadUri = await _firebaseStorage
            .Child(USER_PHOTOS_CONTAINER)
            .Child(fileName)
            .PutAsync(imageStream);

        return downloadUri;
    }
}